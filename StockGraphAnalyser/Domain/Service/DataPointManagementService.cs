﻿

namespace StockGraphAnalyser.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
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

        public IEnumerable<DataPoints> FindAll(Company.ConstituentOfIndex[] indexes) {
            return this.repository.FindAll(indexes);
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

            if (fullDataPointsSet.Any())
            {
                var startTime = DateTime.Now;
                var fullyProcessedData = this.AddProcessedData(fullDataPointsSet.OrderBy(d => d.Date), dateToInsertFrom);
                Debug.WriteLine("{0} DATAPOINTS NUMBER CRUNCHING TOOK {1}", symbol,
                                DateTime.Now.Subtract(startTime).TotalSeconds);
                this.repository.InsertAll(fullyProcessedData.Where(d => d.Date >= dateToInsertFrom));
            }
        }


        public void FillInMissingProcessedData(string symbol) {
            var datapoints = this.repository.FindAll(symbol).OrderBy(d => d.Date);
            var lastFullyProcessedDataPoint = datapoints.LastOrDefault(q => q.IsProcessed == 1);
            
            // If there are quotes to process then go for it
            if (lastFullyProcessedDataPoint == null || lastFullyProcessedDataPoint.Date < datapoints.Max(q => q.Date))
            {
                var minDateToUpdateInDb = lastFullyProcessedDataPoint == null ? DateTime.MinValue : lastFullyProcessedDataPoint.Date;
                
                var datapointsToUpdate = this.AddProcessedData(datapoints, minDateToUpdateInDb).ToList();
                this.repository.UpdateAll(datapointsToUpdate);
            }
        }

        /// <summary>Takes in all datapoints and adds all processed data;</summary>
        /// <param name="dataPoints">The data points.</param>
        /// <param name="dateToProcessFrom">The date to process from.</param>
        /// <returns></returns>
        private IEnumerable<DataPoints> AddProcessedData(IEnumerable<DataPoints> dataPoints, DateTime dateToProcessFrom) {
            var closes = new ReadOnlyDictionary<DateTime, decimal>(dataPoints.ToDictionary(q => q.Date, q => q.Close));

            var twoHundredDayMaCalc = this.calculatorFactory.CreateMovingAverageCalculator(closes, 200);
            var fiftyDayMaCalc = this.calculatorFactory.CreateMovingAverageCalculator(closes, 50);
            var twentyDayMaCalc = this.calculatorFactory.CreateMovingAverageCalculator(closes, 20);
            var twentyTwoDayEmaCalc = this.calculatorFactory.CreateExponentialMovingAverageCalculator(closes, 22);
            var twelveDayEmaCalc = this.calculatorFactory.CreateExponentialMovingAverageCalculator(closes, 12);
            var standardDeviationCalc = this.calculatorFactory.CreateStandardDeviationCalculator(closes, 20);
            var onePeriodForceIndexCalc = this.calculatorFactory.CreateForceIndexCalculator(dataPoints.Select(d => new Tuple<DateTime, decimal, long>(d.Date, d.Close, d.Volume))); 

            var twoHundredDayMaTask = twoHundredDayMaCalc.CalculateAsync(dateToProcessFrom);
            var fiftyDayMaTask = fiftyDayMaCalc.CalculateAsync(dateToProcessFrom);
            var twentyDayMaTask = twentyDayMaCalc.CalculateAsync(dateToProcessFrom);
            var standardDeviationTask = standardDeviationCalc.CalculateAsync(dateToProcessFrom);
            var onePeriodForceIndexTask = onePeriodForceIndexCalc.CalculateAsync(dateToProcessFrom);
            var twentyTwoDayEmaTask = twentyTwoDayEmaCalc.CalculateAsync();
            var twelveDayEmaTask = twelveDayEmaCalc.CalculateAsync();

            dataPoints = dataPoints.MapNewDataPoint(twoHundredDayMaTask.Result, (p, d) => p.MovingAverageTwoHundredDay = d);
            dataPoints = dataPoints.MapNewDataPoint(fiftyDayMaTask.Result, (p, d) => p.MovingAverageFiftyDay = d);
            dataPoints = dataPoints.MapNewDataPoint(twentyDayMaTask.Result, (p, d) => p.MovingAverageTwentyDay = d);
            dataPoints = dataPoints.MapNewDataPoint(onePeriodForceIndexTask.Result, (p, d) => p.ForceIndexOnePeriod = d);

            var macdLineCalc = this.calculatorFactory.CreateDifferenceCalculator(twelveDayEmaTask.Result, twentyTwoDayEmaTask.Result);

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

            var macdLineTask = macdLineCalc.CalculateAsync(dateToProcessFrom);
            var thirteenPeriodForceIndexTask = thirteenPeriodForceIndexCalc.CalculateAsync(dateToProcessFrom);
            var upperTwoDeviationBollingerBandTask = upperTwoDeviationBollingerBandCalc.CalculateAsync(dateToProcessFrom);
            var lowerTwoDeviationBollingerBandTask = lowerTwoDeviationBollingerBandCalc.CalculateAsync(dateToProcessFrom);
            var upperOneDeviationBollingerBandTask = upperOneDeviationBollingerBandCalc.CalculateAsync(dateToProcessFrom);
            var lowerOneDeviationBollingerBandTask = lowerOneDeviationBollingerBandCalc.CalculateAsync(dateToProcessFrom);
            
            dataPoints = dataPoints.MapNewDataPoint(macdLineTask.Result, (p, d) => p.MacdTwentyTwoOverTwelveDay = d);
         
            var macdSignalLineCalc = this.calculatorFactory.CreateExponentialMovingAverageCalculator(dataPoints.Where(d => d.MacdTwentyTwoOverTwelveDay.HasValue).ToDictionary(d => d.Date, d => d.MacdTwentyTwoOverTwelveDay.Value), 9);
            var macdSignalLineTask = macdSignalLineCalc.CalculateAsync(dateToProcessFrom);
            
            dataPoints = dataPoints.MapNewDataPoint(macdSignalLineTask.Result, (p, d) => p.MacdTwentyTwoOverTwelveDaySignalLine = d);
            
            var macdHistogramCalc = this.calculatorFactory.CreateDifferenceCalculator(
                dataPoints.Where(d => d.MacdTwentyTwoOverTwelveDay.HasValue).ToDictionary(d => d.Date, d => d.MacdTwentyTwoOverTwelveDay.Value),
                dataPoints.Where(d => d.MacdTwentyTwoOverTwelveDaySignalLine.HasValue).ToDictionary(d => d.Date, d => d.MacdTwentyTwoOverTwelveDaySignalLine.Value));
            var macdHistogramTask = macdHistogramCalc.CalculateAsync(dateToProcessFrom);
                    
            dataPoints = dataPoints.MapNewDataPoint(lowerTwoDeviationBollingerBandTask.Result, (p, d) => p.LowerBollingerBandTwoDeviation = d);
            dataPoints = dataPoints.MapNewDataPoint(upperTwoDeviationBollingerBandTask.Result, (p, d) => p.UpperBollingerBandTwoDeviation = d);
            dataPoints = dataPoints.MapNewDataPoint(lowerOneDeviationBollingerBandTask.Result, (p, d) => p.LowerBollingerBandOneDeviation = d);
            dataPoints = dataPoints.MapNewDataPoint(upperOneDeviationBollingerBandTask.Result, (p, d) => p.UpperBollingerBandOneDeviation = d); 
            dataPoints = dataPoints.MapNewDataPoint(thirteenPeriodForceIndexTask.Result, (p, d) => p.ForceIndexThirteenPeriod = d);
            dataPoints = dataPoints.MapNewDataPoint(macdHistogramTask.Result, (p, d) => p.MacdTwentyTwoOverTwelveDayHistogram = d);
            
            return dataPoints;
        }
    }
}
