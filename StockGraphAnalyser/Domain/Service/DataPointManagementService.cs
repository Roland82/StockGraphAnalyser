

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

        public DataPointManagementService(IDataPointRepository repository, IYahooStockQuoteServiceClient stockQuoteClient, ICalculatorFactory calculatorFactory) {
            this.repository = repository;
            this.stockQuoteClient = stockQuoteClient;
            this.calculatorFactory = calculatorFactory;
        }


        /// <summary>Inserts the new quotes into database.</summary>
        /// <param name="symbol">The symbol.</param>
        public void InsertNewQuotesToDb(string symbol)
        {
            var quotes = this.stockQuoteClient.GetQuotes(symbol).Where(q => q.Date > DateTime.Today.AddYears(-2));
            var maxDataInDb = this.repository.FindLatestDataPointDateForSymbol(symbol);
            var dataPointsToInsert = quotes.Where(q => q.Date > maxDataInDb).Select(DataPoints.CreateFromQuote);
            if (dataPointsToInsert.Any())
            {
                this.repository.InsertAll(dataPointsToInsert);
            }
        }


        public void FillInMissingProcessedData(string symbol) {
            var datapoints = this.repository.FindAll(symbol).OrderBy(d => d.Date);
            var lastFullyProcessedDataPoint = datapoints.LastOrDefault(q => q.IsProcessed);
            
            // If there are quotes to process then go for it
            if (lastFullyProcessedDataPoint == null || lastFullyProcessedDataPoint.Date < datapoints.Max(q => q.Date))
            {
                var minDateToUpdateInDb = lastFullyProcessedDataPoint == null ? DateTime.MinValue : lastFullyProcessedDataPoint.Date;
                var datapointsToUpdate = this.AddProcessedData(datapoints, minDateToUpdateInDb).Where(d => d.Date > minDateToUpdateInDb);
                this.repository.UpdateAll(datapointsToUpdate);
            }
        }


        /// <summary>Takes in all datapoints and adds all processed data;</summary>
        /// <param name="dataPoints">The data points.</param>
        /// <param name="dateToProcessFrom">The date to process from.</param>
        /// <returns></returns>
        private IEnumerable<DataPoints> AddProcessedData(IEnumerable<DataPoints> dataPoints, DateTime dateToProcessFrom) {

            var twoHundredDayMa = this.calculatorFactory.CreateMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 200);
            var fiftyDayMa = this.calculatorFactory.CreateMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 50);
            var twentyDayMa = this.calculatorFactory.CreateMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 20);
            var standardDeviationCalculator = this.calculatorFactory.CreateStandardDeviationCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 20);
            var onePeriodForceIndexCalculator = this.calculatorFactory.CreateForceIndexCalculator(dataPoints.Select(d => new Tuple<DateTime, decimal, long>(d.Date, d.Close, d.Volume)));

            // Bollinger bands         
            var twentyDayMaCalc = twentyDayMa.CalculateAsync(dateToProcessFrom);
            var twoHundredDayMaTask = twoHundredDayMa.CalculateAsync(dateToProcessFrom);
            var fiftyDayMaTask = fiftyDayMa.CalculateAsync(dateToProcessFrom);
            var standardDeviationTask = standardDeviationCalculator.CalculateAsync(dateToProcessFrom);
            var onePeriodForceIndexTask = onePeriodForceIndexCalculator.CalculateAsync(dateToProcessFrom);
            
            var twentyDayMaResult = twentyDayMaCalc.Result;
            var standardDeviationResult = standardDeviationTask.Result;
            var forceIndexOnePeriodResult = onePeriodForceIndexTask.Result;

            var upperBollingerBand = this.calculatorFactory.CreateBollingerBandCalculator(twentyDayMaResult, standardDeviationResult, BollingerBandCalculator.Band.Upper);
            var lowerBollingerBand = this.calculatorFactory.CreateBollingerBandCalculator(twentyDayMaResult, standardDeviationResult, BollingerBandCalculator.Band.Lower);
            var thirteenPeriodForceIndex = this.calculatorFactory.CreateExponentialMovingAverageCalculator(forceIndexOnePeriodResult.ToDictionary(f => f.Key, f => f.Value), 13);
            
            var thirteenPeriodForceIndexTask = thirteenPeriodForceIndex.CalculateAsync(dateToProcessFrom);
            var upperBollingerBandTask = upperBollingerBand.CalculateAsync(dateToProcessFrom);
            var lowerBollingerBandTask = lowerBollingerBand.CalculateAsync(dateToProcessFrom);

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
