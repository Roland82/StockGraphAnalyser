
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

        [TestCase(SentimentType.Bearish, true)]
        [TestCase(SentimentType.Bullish, false)]
        public void CurrentDayEngulfsPreviousDayBearish(SentimentType type, bool latestOccurenceHasValue)
        {
            var quotes = new List<Quote>
                {
                    Quote.Create("TST.L", monday.AddDays(0), 10, 12, 15, 9, 0),
                    Quote.Create("TST.L", monday.AddDays(1), 13, 9, 15, 9, 0),
                    Quote.Create("TST.L", monday.AddDays(2), 13, 14, 60, 1, 0),
                    Quote.Create("TST.L", monday.AddDays(3), 14, 15, 60, 1, 0),
                    Quote.Create("TST.L", monday.AddDays(4), 16, 15, 60, 1, 0),
                    Quote.Create("TST.L", monday.AddDays(5), 12, 11, 60, 1, 0),
                };

            var latestOccurencesExpected = new DateTime[] { };
            if (latestOccurenceHasValue) latestOccurencesExpected = new[] { monday.AddDays(1) };

            this.Test(quotes, type, latestOccurencesExpected);
        }

        [TestCase(SentimentType.Bearish, false)]
        [TestCase(SentimentType.Bullish, true)]
        public void CurrentDayEngulfsPreviousDayBullish(SentimentType type, bool latestOccurenceHasValue)
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

            var latestOccurencesExpected = new DateTime[] {};
            if (latestOccurenceHasValue) latestOccurencesExpected = new[] {monday.AddDays(5)};

            this.Test(quotes, type, latestOccurencesExpected);
        }

        private void Test(IEnumerable<Quote> quotes, SentimentType type, IEnumerable<DateTime> expectedOccurences)
        {
            var patternRecogniser = new EngulfingPatterRecogniser(quotes.Select(DataPoints.CreateFromQuote), type);
            var latestOccurences = patternRecogniser.FindOccurences();
            Assert.AreEqual(expectedOccurences, latestOccurences);
        }
    }
}
