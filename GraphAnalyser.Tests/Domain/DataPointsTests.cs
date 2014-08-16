
namespace GraphAnalyser.Tests.Domain
{
    using System;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;

    class DataPointsTests
    {
        [TestCase(100, 90, -10)]
        [TestCase(90, 100, 11.11)]
        [TestCase(100, 100, 0)]
        public void PercentageOpenToCloseTest(decimal open, decimal close, decimal percentage)
        {
            var quote = Quote.Create("Blah", DateTime.Today, open, close, 1000m, 0m, 100);
            Assert.AreEqual(percentage, DataPoints.CreateFromQuote(quote).PercentageChange);
        }
    }
}
