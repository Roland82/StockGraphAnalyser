

namespace GraphAnalyser.Tests.Processing.Types
{
    using System;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Types;

    [TestFixture]
    public class QuoteTests
    {
        [TestCase(100, 90, -10)]
        [TestCase(90, 100, 11.11)]
        [TestCase(100, 100, 0)]
        public void PercentageOpenToCloseTest(decimal open, decimal close, decimal percentage) {
            var quote = Quote.Create("Blah", DateTime.Today, open, close, 1000m, 0m, 100);
            Assert.AreEqual(percentage, quote.OpenToClosePercentageMovement);
        }
    }
}
