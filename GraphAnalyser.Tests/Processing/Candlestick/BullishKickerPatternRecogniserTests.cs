

namespace GraphAnalyser.Tests.Processing.Candlestick
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Processing.Candlestick;

    [TestFixture]
    public class BullishKickerPatternRecogniserTests
    {
        [Test]
        public void PatternIsFoundTest() {
            var datapoints = new List<DataPoints>
                {
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today, 5, 4, 6, 3, 100000)),
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today.AddDays(1), 4, 3, 5, 2, 100000)),
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today.AddDays(2), 3, 2, 4, 1, 100000)),
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today.AddDays(3), 4.1m, 4.8m, 6, 3, 100000))
                };

            var signalDetector = new KickerPatternRecogniser(datapoints);
            var expectedDates = new[] {DateTime.Today.AddDays(3)};
            var actualDates = signalDetector.FindOccurences();
            Assert.AreEqual(expectedDates, actualDates);
        }
    }
}
