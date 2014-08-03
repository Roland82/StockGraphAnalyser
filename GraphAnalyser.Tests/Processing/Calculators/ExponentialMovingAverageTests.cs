namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;
    using TestUtilities;

    public class ExponentialMovingAverageTests
    {
        private readonly DateTime startDate = new DateTime(2010, 3, 24);
        private readonly DateTime[] daysToSkip = new[] { new DateTime(2010, 4, 2) };
        private readonly Dictionary<DateTime, decimal> closingPrices;
 
        public ExponentialMovingAverageTests() {         
            this.closingPrices = GraphPlottingUtilities.CreateGraph(this.startDate, new[] {
                    22.27m, 22.19m, 22.08m, 22.17m, 22.18m, 22.13m, 22.23m, 22.43m, 
                    22.24m, 22.29m, 22.15m, 22.39m, 22.38m, 22.61m, 23.36m, 24.05m
                }, daysToSkip);
        }
            
        [Test]
        public void EmaTest() {
            var expectedResult = GraphPlottingUtilities.CreateGraph(this.startDate.AddDays(14), new[] {
                    22.22m, 22.21m, 22.24m, 22.27m, 22.33m, 22.52m, 22.80m
                }, daysToSkip);

            var emaCalculator = new ExponentialMovingAverageCalculator(closingPrices, 10);
            var result = emaCalculator.Calculate().Result;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void PartialEmaTest()
        {
            var expectedResult = GraphPlottingUtilities.CreateGraph(this.startDate.AddDays(16), new[] { 22.24m, 22.27m, 22.33m, 22.52m, 22.80m}, daysToSkip);
            
            var emaCalculator = new ExponentialMovingAverageCalculator(closingPrices, 10);
            var result = emaCalculator.Calculate(this.startDate.AddDays(16)).Result;
            Assert.AreEqual(expectedResult, result);
        }
    }
}