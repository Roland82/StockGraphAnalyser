
namespace GraphAnalyser.Tests.Processing.Candlestick
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Processing.Candlestick;

    [TestFixture]
    public class EngulfingPatternRecogniserTests
    {
        private DateTime monday = new DateTime(2014, 8, 4);

        [TestCase(EngulfingPatterRecogniser.Type.Bearish, true)]
        [TestCase(EngulfingPatterRecogniser.Type.Bullish, false)]
        public void CurrentDayEngulfsPreviousDayBearish(EngulfingPatterRecogniser.Type type, bool latestOccurenceHasValue) {
            var quotes = new List<Quote>
                {
                    Quote.Create("TST.L", monday.AddDays(0), 10, 12, 15, 9, 0),
                    Quote.Create("TST.L", monday.AddDays(1), 13, 9, 15, 9, 0),
                    Quote.Create("TST.L", monday.AddDays(2), 13, 14, 60, 1, 0),
                    Quote.Create("TST.L", monday.AddDays(3), 14, 15, 60, 1, 0),
                    Quote.Create("TST.L", monday.AddDays(4), 16, 15, 60, 1, 0),
                    Quote.Create("TST.L", monday.AddDays(5), 12, 11, 60, 1, 0),
                };

            DateTime? latestOccurenceExpected = null;
            if (latestOccurenceHasValue) latestOccurenceExpected = monday.AddDays(1);

            this.Test(quotes, type, latestOccurenceExpected);
        }

        [TestCase(EngulfingPatterRecogniser.Type.Bearish, false)]
        [TestCase(EngulfingPatterRecogniser.Type.Bullish, true)]
        public void CurrentDayEngulfsPreviousDayBullish(EngulfingPatterRecogniser.Type type, bool latestOccurenceHasValue)
        {
            var quotes = new List<Quote>
                {
                    Quote.Create("TST.L", monday.AddDays(0), 10, 12, 15, 9, 0),
                    Quote.Create("TST.L", monday.AddDays(1), 11, 13, 15, 9, 0),
                    Quote.Create("TST.L", monday.AddDays(2), 13, 14, 60, 1, 0),
                    Quote.Create("TST.L", monday.AddDays(3), 14, 15, 60, 1, 0),
                    Quote.Create("TST.L", monday.AddDays(4), 16, 15, 60, 1, 0),
                    Quote.Create("TST.L", monday.AddDays(5), 11, 17, 60, 1, 0),
                };

            DateTime? latestOccurenceExpected = null;
            if (latestOccurenceHasValue) latestOccurenceExpected = monday.AddDays(5);

            this.Test(quotes, type, latestOccurenceExpected);
        }

        private void Test(IEnumerable<Quote> quotes, EngulfingPatterRecogniser.Type type, DateTime? expectedDayPatternRecognised) {
            var patternRecogniser = new EngulfingPatterRecogniser(quotes.Select(DataPoints.CreateFromQuote), type);
            var latestOccurence = patternRecogniser.LatestOccurence();
            Assert.AreEqual(expectedDayPatternRecognised.HasValue, latestOccurence.HasValue);

            if (latestOccurence.HasValue)
            {
                Assert.AreEqual(expectedDayPatternRecognised.Value, latestOccurence.Value);
            }
        }
    }
}
