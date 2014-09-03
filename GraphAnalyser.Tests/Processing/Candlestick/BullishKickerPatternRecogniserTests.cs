

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
        public void BullishPatternIsFoundTest() {
            var datapoints = new List<DataPoints>
                {
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today, 5, 4, 6, 3, 100000)),
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today.AddDays(1), 4, 3, 5, 2, 100000)),
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today.AddDays(2), 3, 2, 4, 1, 100000)),
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today.AddDays(3), 4.1m, 4.8m, 6, 3, 100000))
                };

            var signalDetector = new KickerPatternRecogniser(datapoints, SentimentType.Bullish);
            var expectedDates = new[] {DateTime.Today.AddDays(3)};
            var actualDates = signalDetector.FindOccurences();
            Assert.AreEqual(expectedDates, actualDates);
        }

        [Test]
        public void BearishPatternIsFoundTest()
        {
            var datapoints = new List<DataPoints>
                {
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today, 2, 3, 6, 3, 100000)),
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today.AddDays(1), 3, 4, 5, 2, 100000)),
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today.AddDays(2), 4, 5, 5, 4, 100000)),
                    DataPoints.CreateFromQuote(Quote.Create("TST.L", DateTime.Today.AddDays(3), 3.9m, 2.1m, 6, 3, 100000))
                };

            var signalDetector = new KickerPatternRecogniser(datapoints, SentimentType.Bearish);
            var expectedDates = new[] { DateTime.Today.AddDays(3) };
            var actualDates = signalDetector.FindOccurences();
            Assert.AreEqual(expectedDates, actualDates);
        }
    }
}
