

namespace StockGraphAnalyser.Processing.Calculators.Indicators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Detects when two different graphs crossover
    /// </summary>
    public class GraphCrossoverDetector
    {
        private readonly Dictionary<DateTime, decimal> graph1;
        private readonly Dictionary<DateTime, decimal> graph2;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphCrossoverDetector"/> class.
        /// </summary>
        /// <param name="graph1">The moving average50 day calculator.</param>
        /// <param name="graph2">The moving average20 day calculator.</param>
        public GraphCrossoverDetector(Dictionary<DateTime, decimal> graph1, Dictionary<DateTime, decimal> graph2)
        {
            this.graph1 = graph1.OrderBy(d => d.Key).ToDictionary(k => k.Key, k => k.Value);
            this.graph2 = graph2.OrderBy(d => d.Key).ToDictionary(k => k.Key, k => k.Value);
        }

        /// <summary>
        /// Calculates the last day when a crossover or touching price is spotted between two graphs.
        /// </summary>
        /// <returns></returns>
        public DateTime? Calculate() {
            var sharedRange = Utilities.CalculateSharedDateRange(graph1.Select(s => s.Key), graph2.Select(s => s.Key));
            if (sharedRange != null)
            {
                var numberOfPrices = (int)(sharedRange.Item2 - sharedRange.Item1).TotalDays;
                if (numberOfPrices > 1)
                {
                    var graph1Subset = graph1.SubsetBetween(sharedRange.Item1, sharedRange.Item2);
                    var graph2Subset = graph2.SubsetBetween(sharedRange.Item1, sharedRange.Item2);
                    var priceCount = graph1Subset.Count;

                    for (var i = priceCount - 1; i > 0; i--)
                    {
                        // Prices touch
                        if (graph1Subset.ElementAt(i).Value == graph2Subset.ElementAt(i).Value) return graph1Subset.ElementAt(i).Key;

                        // Graph one crosses up over graph two
                        if (((graph1Subset.ElementAt(i).Value > graph2Subset.ElementAt(i).Value) && (graph1Subset.ElementAt(i - 1).Value < graph2Subset.ElementAt(i - 1).Value)))
                        {
                            return graph2Subset.ElementAt(i).Key;
                        }

                        // Graph one crosses down under graph two
                        if (((graph1Subset.ElementAt(i).Value < graph2Subset.ElementAt(i).Value) && (graph1Subset.ElementAt(i - 1).Value > graph2Subset.ElementAt(i - 1).Value)))
                        {
                            return graph2Subset.ElementAt(i).Key;
                        }
                    }
                }
            }


            return null;
        }
    }
}
