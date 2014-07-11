
namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;
    using StockGraphAnalyser.Processing.Types;

    /// <summary>
    /// Wilder started with a concept called True Range (TR), which is defined as the greatest of the following:
    /// Method 1: Current High less the current Low
    /// Method 2: Current High less the previous Close (absolute value)
    /// Method 3: Current Low less the previous Close (absolute value)
    /// </summary>
    public class AverageTrueRangeCalculatorTests
    {

        [Test]
        public void CurrentHighMinusCurrentLowUsedTest()
        {
            var quoteList = new []
                {
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-1), open: 1, close: 2, high: 100, low: 0),
                    Quote.Create("SGP.L", DateTime.Today, open: 2, close: 4, high: 100, low: 0)
                };

            var calculator = new AverageTrueRangeCalculator(quoteList, 2);
            var atrs = calculator.Calculate().Result;
            Assert.AreEqual(1, atrs.Count);
            Assert.AreEqual(100m, atrs.ElementAt(0).Value);
        }

        [Test]
        public void CurrentHighMinusPreviousCloseUsedTest()
        {
            var quoteList = new[]
                {
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-1), open: 1, close: 2, high: 100, low: 0),
                    Quote.Create("SGP.L", DateTime.Today, open: 2, close: 4, high: 100, low: 99)
                };

            var calculator = new AverageTrueRangeCalculator(quoteList, 2);
            var atrs = calculator.Calculate().Result;
            Assert.AreEqual(1, atrs.Count);
            Assert.AreEqual(99m, atrs.ElementAt(0).Value);
        }

        [Test]
        public void CurrentLowMinusPreviousCloseUsedTest()
        {
            var quoteList = new[]
                {
                    Quote.Create("SGP.L", DateTime.Today.AddDays(-1), open: 100, close: 102, high: 200, low: 0),
                    Quote.Create("SGP.L", DateTime.Today, open: 2, close: 4, high: 50, low: 48)
                };

            var calculator = new AverageTrueRangeCalculator(quoteList, 2);
            var atrs = calculator.Calculate().Result;
            Assert.AreEqual(1, atrs.Count);
            Assert.AreEqual(127m, atrs.ElementAt(0).Value);
        }
    }
}
