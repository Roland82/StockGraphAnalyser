

namespace StockGraphAnalyser.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using Processing;
    using Processing.Calculators;
    using Repository.Interfaces;
    using Web.Interfaces;
    using System.Linq;

    public class DataPointManagementService
    {
        private readonly IDataPointRepository repository;
        private readonly IYahooStockQuoteServiceClient stockQuoteClient;
        private readonly ICalculatorFactory calculatorFactory;
        private const short MinQuoteRequirementForProcessing = 200;

        public DataPointManagementService(IDataPointRepository repository, IYahooStockQuoteServiceClient stockQuoteClient, ICalculatorFactory calculatorFactory) {
            this.repository = repository;
            this.stockQuoteClient = stockQuoteClient;
            this.calculatorFactory = calculatorFactory;
        }

        public void FillInMissingProcessedData(string symbol) {
            var datapoints = this.repository.FindAll(symbol).OrderBy(d => d.Date);
            var lastFullyProcessedDataPoint = datapoints.OrderByDescending(q => q.Date).FirstOrDefault(q => q.IsProcessed);
            
            // If minimum data is present and there are quotes to process then go for it
            if (lastFullyProcessedDataPoint == null || lastFullyProcessedDataPoint.Date < datapoints.Max(q => q.Date) && datapoints.Count() > MinQuoteRequirementForProcessing)
            {
                var minDateToUpdateInDb = lastFullyProcessedDataPoint == null ? DateTime.MinValue : lastFullyProcessedDataPoint.Date;
                var datapointsToUpdate = this.AddProcessedData(datapoints, minDateToUpdateInDb).Where(d => d.Date > minDateToUpdateInDb);
                this.repository.UpdateAll(datapointsToUpdate);
            }
        }

        public void InsertNewQuotesToDb(string symbol) {
            var quotes = this.stockQuoteClient.GetQuotes(symbol).Where(q => q.Date > DateTime.Today.AddYears(-2));
            var maxDataInDb = this.repository.FindLatestDataPointDateForSymbol(symbol);
            var dataPointsToInsert = quotes.Where(q => q.Date > maxDataInDb).Select(DataPoints.CreateFromQuote);
            if (dataPointsToInsert.Any())
            {
                this.repository.InsertAll(dataPointsToInsert);
            }
        }

        /// <summary>
        /// Takes in all datapoints and adds all processed data;
        /// </summary>
        /// <param name="dataPoints"></param>
        /// <returns></returns>
        private IEnumerable<DataPoints> AddProcessedData(IEnumerable<DataPoints> dataPoints, DateTime dateToProcessFrom) {

            var twoHundredDayMa = this.calculatorFactory.CreateMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 200);
            var fiftyDayMa = this.calculatorFactory.CreateMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 50);
            var twentyDayMa = this.calculatorFactory.CreateMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 20);
            var standardDeviationCalculator = this.calculatorFactory.CreateStandardDeviationCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 20);
            var onePeriodForceIndexCalculator = this.calculatorFactory.CreateForceIndexCalculator(dataPoints.Select(d => new Tuple<DateTime, decimal, long>(d.Date, d.Close, d.Volume)));

            // Bollinger bands         
            var twentyDayMaCalc = twentyDayMa.Calculate(dateToProcessFrom);
            var twoHundredDayMaTask = twoHundredDayMa.Calculate(dateToProcessFrom);
            var fiftyDayMaTask = fiftyDayMa.Calculate(dateToProcessFrom);
            var standardDeviationTask = standardDeviationCalculator.Calculate(dateToProcessFrom);
            var onePeriodForceIndexTask = onePeriodForceIndexCalculator.Calculate(dateToProcessFrom);
            
            var twentyDayMaResult = twentyDayMaCalc.Result;
            var standardDeviationResult = standardDeviationTask.Result;
            var forceIndexOnePeriodResult = onePeriodForceIndexTask.Result;

            var upperBollingerBand = this.calculatorFactory.CreateBollingerBandCalculator(twentyDayMaResult, standardDeviationResult, BollingerBandCalculator.Band.Upper);
            var lowerBollingerBand = this.calculatorFactory.CreateBollingerBandCalculator(twentyDayMaResult, standardDeviationResult, BollingerBandCalculator.Band.Lower);
            var thirteenPeriodForceIndex = this.calculatorFactory.CreateExponentialMovingAverageCalculator(forceIndexOnePeriodResult.ToDictionary(f => f.Key, f => f.Value), 13);
            
            var thirteenPeriodForceIndexTask = thirteenPeriodForceIndex.Calculate(dateToProcessFrom);
            var upperBollingerBandTask = upperBollingerBand.Calculate(dateToProcessFrom);
            var lowerBollingerBandTask = lowerBollingerBand.Calculate(dateToProcessFrom);

            dataPoints = dataPoints.MapNewDataPoint(twoHundredDayMaTask.Result, (p, d) => p.MovingAverageTwoHundredDay = d);
            dataPoints = dataPoints.MapNewDataPoint(fiftyDayMaTask.Result, (p, d) => p.MovingAverageFiftyDay = d);
            dataPoints = dataPoints.MapNewDataPoint(lowerBollingerBandTask.Result, (p, d) => p.LowerBollingerBand = d);
            dataPoints = dataPoints.MapNewDataPoint(upperBollingerBandTask.Result, (p, d) => p.UpperBollingerBand = d);
            dataPoints = dataPoints.MapNewDataPoint(onePeriodForceIndexTask.Result, (p, d) => p.ForceIndexOnePeriod = d);
            dataPoints = dataPoints.MapNewDataPoint(thirteenPeriodForceIndexTask.Result, (p, d) => p.ForceIndexThirteenPeriod = d);
            dataPoints = dataPoints.UpdateAll(x => x.IsProcessed = true);
            return dataPoints;
        }
    }
}
