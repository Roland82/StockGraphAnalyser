

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
        private readonly Dictionary<ICalculationTool, Action<DataPoints, decimal>> processorJobs;
        private const short MinQuoteRequirementForProcessing = 200;

        public DataPointManagementService(IDataPointRepository repository, IYahooStockQuoteServiceClient stockQuoteClient) {
            this.repository = repository;
            this.stockQuoteClient = stockQuoteClient;
        }

        public void FillInMissingProcessedData(string symbol) {
            var datapoints = this.repository.FindAll(symbol).OrderBy(d => d.Date);
            var lastFullyProcessedDataPoint = datapoints.OrderByDescending(q => q.Date).FirstOrDefault(q => q.IsProcessed);
            
            // If minimum data is present and there are quotes to process then go for it
            if (lastFullyProcessedDataPoint == null || lastFullyProcessedDataPoint.Date < datapoints.Max(q => q.Date) && datapoints.Count() > MinQuoteRequirementForProcessing)
            {
                var minDateToUpdateInDb = lastFullyProcessedDataPoint == null ? DateTime.MinValue : lastFullyProcessedDataPoint.Date;
                var datapointsToUpdate = this.AddProcessedData(datapoints).Where(d => d.Date > minDateToUpdateInDb);
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
        private IEnumerable<DataPoints> AddProcessedData(IEnumerable<DataPoints> dataPoints) {

            var twoHundredDayMa = new DailyMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 200);
            var fiftyDayMa = new DailyMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 50);
            var twentyDayMa = new DailyMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 20);
            var standardDeviationCalculator = new StandardDeviationCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close));
            var onePeriodForceIndexCalculator = new ForceIndexCalculator(dataPoints.Select(d => new Tuple<DateTime, decimal, long>(d.Date, d.Close, d.Volume)));


            // Bollinger bands         
            var twentyDayMaCalc = twentyDayMa.Calculate();
            var twoHundredDayMaTask = twoHundredDayMa.Calculate();
            var fiftyDayMaTask = fiftyDayMa.Calculate();
            var standardDeviationTask = standardDeviationCalculator.Calculate();
            var onePeriodForceIndexTask = onePeriodForceIndexCalculator.Calculate();
            var twentyDayMaResult = twentyDayMaCalc.Result;
            var standardDeviationResult = standardDeviationTask.Result;
            var forceIndexOnePeriodResult = onePeriodForceIndexTask.Result;

            var upperBollingerBandTask = new BollingerBandCalculator(twentyDayMaResult, standardDeviationResult, BollingerBandCalculator.Band.Upper).Calculate();
            var lowerBollingerBandTask = new BollingerBandCalculator(twentyDayMaResult, standardDeviationResult,BollingerBandCalculator.Band.Lower).Calculate();
            var thirteenPeriodForceIndex = new ExponentialMovingAverageCalculator(forceIndexOnePeriodResult.ToDictionary(f => f.Key, f => f.Value), 13);
            var thirteenPeriodForceIndexTask = thirteenPeriodForceIndex.Calculate();

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
