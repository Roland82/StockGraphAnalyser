

using System;

namespace StockGraphAnalyser.Domain
{
    using System.Collections.Generic;
    /// <summary>
    /// Encapsulates data about stock prices.
    /// </summary>
    public class Graph
    {
        private Dictionary<DateTime, decimal> dailyPrices;

        public Graph(Dictionary<DateTime, decimal> dailyPrices)
        {
            this.dailyPrices = dailyPrices;
        }
    }
}
