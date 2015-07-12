
namespace GraphAnalyser.Tests.Domain.Repository.Cassandra
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository.Cassandra;

    [TestFixture]
    public class DataPointRepositoryTests
    {
        private readonly DataPointRepository dataPointRepository = new DataPointRepository();

        [SetUp]
        public void Setup() {
            ConnectionManager.Instance.Session.Execute("TRUNCATE datapoints");
        }

        [Test]
        public void InsertTest() {
            var dataPoints = new DataPoints();
            dataPoints.Open = 1;
            dataPoints.Close = 2;
            dataPoints.High = 3;
            dataPoints.Low = 4;
            dataPoints.Volume = 5;
            dataPoints.MovingAverageFiftyDay = 6;
            dataPoints.MovingAverageTwentyDay = 7;
            dataPoints.MovingAverageTwoHundredDay = 8;
            dataPoints.ForceIndexOnePeriod = 9;
            dataPoints.ForceIndexThirteenPeriod = 10;
            dataPoints.IsProcessed = 0;
            dataPoints.LowerBollingerBandOneDeviation = 11;
            dataPoints.LowerBollingerBandTwoDeviation = 12;
            dataPoints.UpperBollingerBandOneDeviation = 13;
            dataPoints.UpperBollingerBandTwoDeviation = 14;
            dataPoints.MacdTwentyTwoOverTwelveDay = 15;
            dataPoints.MacdTwentyTwoOverTwelveDayHistogram = 16;
            dataPoints.MacdTwentyTwoOverTwelveDaySignalLine = 17;
            
            dataPoints.Symbol = "TST.L";
            dataPoints.Date = DateTime.Today;
            dataPointRepository.InsertAll(new List<DataPoints>() { dataPoints });
    
            var fetchedDataPoint = dataPointRepository.FindAll("TST.L").Result.ElementAt(0);
            Assert.AreEqual(1, dataPointRepository.FindAll("TST.L").Result.Count());
            Assert.AreEqual(dataPoints, fetchedDataPoint);

        }

        [Test]
        public void UpdateTest()
        {
            var dataPoints = new DataPoints();
            dataPoints.Open = 1;
            dataPoints.Close = 2;
            dataPoints.High = 3;
            dataPoints.Low = 4;
            dataPoints.Volume = 5;
            dataPoints.MovingAverageFiftyDay = 6;
            dataPoints.MovingAverageTwentyDay = 7;
            dataPoints.MovingAverageTwoHundredDay = 8;
            dataPoints.ForceIndexOnePeriod = 9;
            dataPoints.ForceIndexThirteenPeriod = 10;
            dataPoints.IsProcessed = 0;
            dataPoints.LowerBollingerBandOneDeviation = 11;
            dataPoints.LowerBollingerBandTwoDeviation = 12;
            dataPoints.UpperBollingerBandOneDeviation = 13;
            dataPoints.UpperBollingerBandTwoDeviation = 14;
            dataPoints.MacdTwentyTwoOverTwelveDay = 15;
            dataPoints.MacdTwentyTwoOverTwelveDayHistogram = 16;
            dataPoints.MacdTwentyTwoOverTwelveDaySignalLine = 17;

            dataPoints.Symbol = "TST.L";
            dataPoints.Date = DateTime.Today;
            dataPointRepository.InsertAll(new List<DataPoints>() { dataPoints });

            var fetchedDataPoint = dataPointRepository.FindAll("TST.L").Result.ElementAt(0);
            Assert.AreEqual(1, dataPointRepository.FindAll("TST.L").Result.Count());
            Assert.AreEqual(dataPoints, fetchedDataPoint);

            dataPoints.Close = 100;
            dataPointRepository.UpdateAll(new List<DataPoints> { dataPoints });
            fetchedDataPoint = dataPointRepository.FindAll("TST.L").Result.ElementAt(0);
            Assert.AreEqual(1, dataPointRepository.FindAll("TST.L").Result.Count());
            Assert.AreEqual(100, fetchedDataPoint.Close);
            Assert.AreEqual(5, fetchedDataPoint.Volume);
        }
    }
}
