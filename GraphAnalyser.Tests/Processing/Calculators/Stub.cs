using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAnalyser.Tests.Processing.Calculators
{
    using NUnit.Framework;
    using StockGraphAnalyser.Domain.Web;
    using StockGraphAnalyser.Processing.Calculators;

    public class Stub
    {
        [Test]
        public void SuperPoop() {
            var service = new YahooStockQuoteServiceClient();
            var quotes = service.GetQuotes("SGP");
            var emaCalculator = new ExponentialMovingAverageCalculator(quotes.ToDictionary(q => q.Date, q => q.Close), 10);
            var result = emaCalculator.Calculate().Result;
            Assert.AreEqual(result, null);
        }
    }
}
