

namespace StockGraphAnalyser.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Processing;
    using Processing.Calculators;
    using Repository.Interfaces;
    using System.Linq;
    using StockDataProviders.Interfaces;

    public class DataPointManagementService : IDataPointManagementService
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
            var storedDatapoints = this.repository.FindAll(symbol).OrderBy(d => d.Date);
            var dateToInsertFrom = storedDatapoints.Any() ? storedDatapoints.Max(d => d.Date).AddDays(1) : DateTime.Today.AddYears(-2);
            var newDataPoints = this.stockQuoteClient.GetQuotes(symbol).Where(q => q.Date >= dateToInsertFrom).Select(DataPoints.CreateFromQuote);

            var fullDataPointsSet = storedDatapoints.ToList();
            fullDataPointsSet.AddRange(newDataPoints);
            var fullyProcessedData = this.AddProcessedData(fullDataPointsSet.OrderBy(d => d.Date), dateToInsertFrom);
           
            this.repository.InsertAll(fullyProcessedData.Where(d => d.Date >= dateToInsertFrom));    
        }


        public void FillInMissingProcessedData(string symbol) {
            var datapoints = this.repository.FindAll(symbol).OrderBy(d => d.Date);
            var lastFullyProcessedDataPoint = datapoints.LastOrDefault(q => q.IsProcessed == 1);
            
            // If there are quotes to process then go for it
            if (lastFullyProcessedDataPoint == null || lastFullyProcessedDataPoint.Date < datapoints.Max(q => q.Date))
            {
                var minDateToUpdateInDb = lastFullyProcessedDataPoint == null ? DateTime.MinValue : lastFullyProcessedDataPoint.Date;
                var datapointsToUpdate = this.AddProcessedData(datapoints, minDateToUpdateInDb).Where(d => d.Date > minDateToUpdateInDb).ToList();
                this.repository.UpdateAll(datapointsToUpdate);
            }
        }


        /// <summary>Takes in all datapoints and adds all processed data;</summary>
        /// <param name="dataPoints">The data points.</param>
        /// <param name="dateToProcessFrom">The date to process from.</param>
        /// <returns></returns>
        private IEnumerable<DataPoints> AddProcessedData(IEnumerable<DataPoints> dataPoints, DateTime dateToProcessFrom) {

            var twoHundredDayMaCalc = this.calculatorFactory.CreateMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 200);
            var fiftyDayMaCalc = this.calculatorFactory.CreateMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 50);
            var twentyDayMaCalc = this.calculatorFactory.CreateMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 20);
            var standardDeviationCalc = this.calculatorFactory.CreateStandardDeviationCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 20);
            var onePeriodForceIndexCalc = this.calculatorFactory.CreateForceIndexCalculator(dataPoints.Select(d => new Tuple<DateTime, decimal, long>(d.Date, d.Close, d.Volume))); 

            var twoHundredDayMaTask = twoHundredDayMaCalc.CalculateAsync(dateToProcessFrom);
            var fiftyDayMaTask = fiftyDayMaCalc.CalculateAsync(dateToProcessFrom);
            var twentyDayMaTask = twentyDayMaCalc.CalculateAsync(dateToProcessFrom);
            var standardDeviationTask = standardDeviationCalc.CalculateAsync(dateToProcessFrom);
            var onePeriodForceIndexTask = onePeriodForceIndexCalc.CalculateAsync(dateToProcessFrom);           

            dataPoints = dataPoints.MapNewDataPoint(twoHundredDayMaTask.Result, (p, d) => p.MovingAverageTwoHundredDay = d);
            dataPoints = dataPoints.MapNewDataPoint(fiftyDayMaTask.Result, (p, d) => p.MovingAverageFiftyDay = d);
            dataPoints = dataPoints.MapNewDataPoint(twentyDayMaTask.Result, (p, d) => p.MovingAverageTwentyDay = d);
            dataPoints = dataPoints.MapNewDataPoint(onePeriodForceIndexTask.Result, (p, d) => p.ForceIndexOnePeriod = d);

            var upperTwoDeviationBollingerBandCalc = this.calculatorFactory.CreateBollingerBandCalculator(
                dataPoints.Where(d => d.MovingAverageTwentyDay.HasValue).ToDictionary(d => d.Date, d => d.MovingAverageTwentyDay.Value),
                standardDeviationTask.Result, 
                BollingerBandCalculator.Band.UpperTwoDeviation);

            var lowerTwoDeviationBollingerBandCalc = this.calculatorFactory.CreateBollingerBandCalculator(
                dataPoints.Where(d => d.MovingAverageTwentyDay.HasValue).ToDictionary(d => d.Date, d => d.MovingAverageTwentyDay.Value),
                standardDeviationTask.Result,
                BollingerBandCalculator.Band.LowerTwoDeviation);

            var upperOneDeviationBollingerBandCalc = this.calculatorFactory.CreateBollingerBandCalculator(
                dataPoints.Where(d => d.MovingAverageTwentyDay.HasValue).ToDictionary(d => d.Date, d => d.MovingAverageTwentyDay.Value),
                standardDeviationTask.Result,
                BollingerBandCalculator.Band.UpperOneDeviation);

            var lowerOneDeviationBollingerBandCalc = this.calculatorFactory.CreateBollingerBandCalculator(
                dataPoints.Where(d => d.MovingAverageTwentyDay.HasValue).ToDictionary(d => d.Date, d => d.MovingAverageTwentyDay.Value),
                standardDeviationTask.Result,
                BollingerBandCalculator.Band.LowerOneDeviation);

            var thirteenPeriodForceIndexCalc = this.calculatorFactory.CreateExponentialMovingAverageCalculator(
                dataPoints.Where(d => d.ForceIndexOnePeriod.HasValue).ToDictionary(f => f.Date, f => f.ForceIndexOnePeriod.Value), 
                13);
            
            var thirteenPeriodForceIndexTask = thirteenPeriodForceIndexCalc.CalculateAsync(dateToProcessFrom);
            var upperTwoDeviationBollingerBandTask = upperTwoDeviationBollingerBandCalc.CalculateAsync(dateToProcessFrom);
            var lowerTwoDeviationBollingerBandTask = lowerTwoDeviationBollingerBandCalc.CalculateAsync(dateToProcessFrom);
            var upperOneDeviationBollingerBandTask = upperOneDeviationBollingerBandCalc.CalculateAsync(dateToProcessFrom);
            var lowerOneDeviationBollingerBandTask = lowerOneDeviationBollingerBandCalc.CalculateAsync(dateToProcessFrom);

            dataPoints = dataPoints.MapNewDataPoint(lowerTwoDeviationBollingerBandTask.Result, (p, d) => p.LowerBollingerBandTwoDeviation = d);
            dataPoints = dataPoints.MapNewDataPoint(upperTwoDeviationBollingerBandTask.Result, (p, d) => p.UpperBollingerBandTwoDeviation = d);
            dataPoints = dataPoints.MapNewDataPoint(lowerOneDeviationBollingerBandTask.Result, (p, d) => p.LowerBollingerBandOneDeviation = d);
            dataPoints = dataPoints.MapNewDataPoint(upperOneDeviationBollingerBandTask.Result, (p, d) => p.UpperBollingerBandOneDeviation = d); 

            dataPoints = dataPoints.MapNewDataPoint(thirteenPeriodForceIndexTask.Result, (p, d) => p.ForceIndexThirteenPeriod = d);
            dataPoints = dataPoints.UpdateAll(x => x.IsProcessed = 1);
            return dataPoints;
        }
    }
}
