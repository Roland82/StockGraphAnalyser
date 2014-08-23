

namespace GraphAnalyser.Tests.Web
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain.StockDataProviders;


    [TestFixture]
    public class YahooStockQuoteServiceClientTests
    {
        [Test]
        public void WebRequestTest() {
            var client = new YahooStockQuoteServiceClient();
            var quotes = client.GetQuotes("SGP.L").ToList();
            Assert.True(quotes.Any());

            var testQuote = quotes.First(q => q.Date == new DateTime(2014, 5, 2));
            Assert.AreEqual(1377, testQuote.Open);
            Assert.AreEqual(1407, testQuote.Close);
            Assert.AreEqual(1413, testQuote.High);
            Assert.AreEqual(1368, testQuote.Low);
        }
    }
}
