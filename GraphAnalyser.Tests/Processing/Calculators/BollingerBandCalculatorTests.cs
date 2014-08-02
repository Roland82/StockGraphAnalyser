

namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;
    using TestUtilities;

    public class BollingerBandCalculatorTests
    {
        private readonly Dictionary<DateTime, decimal> movingAverage = GraphPlottingUtilities.CreateGraph(DateTime.MinValue, new[] { 88.71m, 89.05m, 89.24m, 89.39m, 89.51m, 89.69m });
        private readonly Dictionary<DateTime, decimal> standardDeviation = GraphPlottingUtilities.CreateGraph(DateTime.MinValue, new[] { 1.29m, 1.45m, 1.69m, 1.77m, 1.90m, 2.02m });
            
        [Test]
        public void UpperBollingerBandWithFullDateRangeTest() {
            var bollingerBandCalculator = new BollingerBandCalculator(movingAverage, standardDeviation,BollingerBandCalculator.Band.Upper);

            var expectedResult = GraphPlottingUtilities.CreateGraph(DateTime.MinValue, new[] {91.29m, 91.95m, 92.62m, 92.93m, 93.31m, 93.73m});
            var actualResult = bollingerBandCalculator.Calculate().Result;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void TestPartialDateRangeProcessing()
        {
            var bollingerBandCalculator = new BollingerBandCalculator(movingAverage, standardDeviation, BollingerBandCalculator.Band.Upper);

            var expectedResult = GraphPlottingUtilities.CreateGraph(DateTime.MinValue.AddDays(2), new[] { 92.62m, 92.93m, 93.31m, 93.73m });
            var actualResult = bollingerBandCalculator.Calculate(DateTime.MinValue.AddDays(2)).Result;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void LowerBollingerBandTest()
        {
            var bollingerBandCalculator = new BollingerBandCalculator(movingAverage, standardDeviation, BollingerBandCalculator.Band.Lower);

            var expectedResult = GraphPlottingUtilities.CreateGraph(DateTime.MinValue, new[] { 86.13m, 86.15m, 85.86m, 85.85m, 85.71m, 85.65m });
            var actualResult = bollingerBandCalculator.Calculate().Result;
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
