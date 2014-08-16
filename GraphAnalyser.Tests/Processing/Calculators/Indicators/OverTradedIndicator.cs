

namespace GraphAnalyser.Tests.Processing.Calculators.Indicators
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using StockGraphAnalyser.Domain;

    [TestFixture]
    public class OverTradedIndicator
    {
        private Dictionary<DateTime, decimal> movingAverages;
        private IEnumerable<Quote> quotes;

        public OverTradedIndicator(Dictionary<DateTime, decimal> movingAverages, IEnumerable<Quote> quotes) {
            this.movingAverages = movingAverages;
            this.quotes = quotes;
        }


    }
}
