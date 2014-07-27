

namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;
    using StockGraphAnalyser.Processing.Types;

    [TestFixture]
    public class DailyMovingAverageCalculatorTests
    {
        [Test]
        public void DailyMovingAverageCalculation() {
            var quoteList = new[]
                {
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-6), open: 1, close: 11, high: 100, low: 0, volume: 100),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-5), open: 2, close: 12, high: 100, low: 0, volume: 100),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-4), open: 4, close: 13, high: 100, low: 0, volume: 100),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-3), open: 8, close: 14, high: 100, low: 0, volume: 100),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-2), open: 16, close: 15, high: 100, low: 0, volume: 100),
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-1), open: 32, close: 16, high: 100, low: 0, volume: 100),
                    Quote.Create("SGP.L", DateTime.Today, open: 64, close: 17, high: 100, low: 0, volume: 100)
                };
        
            var expectedResult = new Dictionary<DateTime, decimal>{
                                                                      {DateTime.Today.AddDays(-2), 13m},
                                                                      {DateTime.Today.AddDays(-1), 14m},
                                                                      {DateTime.Today, 15m}
                                                                  };

            var calculator = new DailyMovingAverageCalculator(quoteList.ToDictionary(q => q.Date, q => q.Close), 5);
            var result = calculator.Calculate().Result;
            Assert.AreEqual(
                expectedResult,
                result,
                string.Format("Expected: {0} Actual: {1}", expectedResult, result));
        }
    }
}
