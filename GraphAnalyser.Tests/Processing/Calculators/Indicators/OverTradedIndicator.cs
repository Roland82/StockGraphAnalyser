

namespace GraphAnalyser.Tests.Processing.Calculators.Indicators
{
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Types;
    using System;
    using System.Collections.Generic;

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
