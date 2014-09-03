
namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Processing.Calculators;

    [TestFixture]
    public class TrueRangeCalculatorTests
    {
        [Test]
        public void Test()
        {
            var quotes = new[]  {
                                      Quote.Create("TEST.L", DateTime.Today, 0, 44.52m, 44.53m, 43.98m, 1000),
                                      Quote.Create("TEST.L", DateTime.Today.AddDays(1), 0, 44.65m, 44.93m, 44.36m, 1000),
                                      Quote.Create("TEST.L", DateTime.Today.AddDays(2), 0, 45.22m, 45.39m, 44.70m, 1000),
                                      Quote.Create("TEST.L", DateTime.Today.AddDays(3), 0, 45.45m, 45.70m, 45.13m, 1000),
                                      Quote.Create("TEST.L", DateTime.Today.AddDays(4), 0, 45.49m, 45.63m, 44.89m, 1000)
                                    };
            var calculator = new TrueRangeCalculator(quotes.Select(DataPoints.CreateFromQuote));
            var result = calculator.CalculateAsync().Result;
            var expectedResult = new Dictionary<DateTime, decimal>{
                                                                      {DateTime.Today.AddDays(1), 0.57m},
                                                                      {DateTime.Today.AddDays(2), 0.74m},
                                                                      {DateTime.Today.AddDays(3), 0.57m},
                                                                      {DateTime.Today.AddDays(4), 0.74m}
                                                                  };
            Assert.AreEqual(expectedResult, result);
        }
    }
}
