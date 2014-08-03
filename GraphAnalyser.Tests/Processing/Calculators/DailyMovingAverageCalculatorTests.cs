

namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;
    using TestUtilities;

    [TestFixture]
    public class DailyMovingAverageCalculatorTests
    {
        private readonly DateTime monday = new DateTime(2014, 8, 4);

        [Test]
        public void DailyMovingAverageCalculation() {
            var closingPrices = GraphPlottingUtilities.CreateGraph(monday, new[] {11m, 12m, 13m, 14m, 15m, 16m, 100m, 90m, 80m});
            var expectedResult = GraphPlottingUtilities.CreateGraph(monday.AddDays(4), new[] { 13m, 14m, 31.6m, 47, 60.2m });
            var calculator = new DailyMovingAverageCalculator(closingPrices, 5);
            var result = calculator.CalculateAsync().Result;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void DailyMovingAverageLastDayCalculationOnly()
        {
            var closingPrices = GraphPlottingUtilities.CreateGraph(monday, new[] { 11m, 12m, 13m, 14m, 15m, 16m, 100m, 90m, 80m });
            var finalDay = closingPrices.Last().Key;
            var expectedResult = GraphPlottingUtilities.CreateGraph(finalDay, new[] { 60.2m });
            var calculator = new DailyMovingAverageCalculator(closingPrices, 5);
            var result = calculator.CalculateAsync(finalDay).Result;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void DailyMovingAveragePartialRange()
        {
            var closingPrices = GraphPlottingUtilities.CreateGraph(monday, new[] { 11m, 12m, 13m, 14m, 15m, 16m, 100m, 90m, 80m });
            var finalDay = closingPrices.ElementAt(6).Key;
            var expectedResult = GraphPlottingUtilities.CreateGraph(finalDay, new[] { 31.6m, 47, 60.2m });
            var calculator = new DailyMovingAverageCalculator(closingPrices, 5);
            var result = calculator.CalculateAsync(finalDay).Result;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void DailyMovingAverageDayToUpdateFromIsWeekend()
        {
            var closingPrices = GraphPlottingUtilities.CreateGraph(monday, new[] { 11m, 12m, 13m, 14m, 15m, 16m, 100m, 90m, 80m });
            var expectedPrices = GraphPlottingUtilities.CreateGraph(monday.AddDays(7), new[] {14m, 31.6m, 47, 60.2m});
            var calculator = new DailyMovingAverageCalculator(closingPrices, 5);
            var result = calculator.CalculateAsync(monday.AddDays(5)).Result;
            Assert.AreEqual(expectedPrices, result);
        }

        [Test]
        public void DailyMovingAverageDayToCalculateIsAheadOfLatestPriceData()
        {
            var closingPrices = GraphPlottingUtilities.CreateGraph(monday, new[] { 11m, 12m, 13m, 14m, 15m, 16m, 100m, 90m, 80m });
            var calculator = new DailyMovingAverageCalculator(closingPrices, 5);
            var result = calculator.CalculateAsync(DateTime.Today.AddYears(10)).Result;
            Assert.AreEqual(new Dictionary<DateTime, decimal>(), result);
        }
    }
}
