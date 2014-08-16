
namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Given candlestick price data and two price based charts, this calculator works out
    /// how much the price data stays within those two graphs.
    /// </summary>
    public class RangePredictabilityCalculator
    {
        private readonly IEnumerable<Tuple<DateTime, decimal, decimal>> opensAndCloses;
        private readonly Dictionary<DateTime, decimal> range1;
        private readonly Dictionary<DateTime, decimal> range2;

        public RangePredictabilityCalculator(IEnumerable<Tuple<DateTime, decimal, decimal>> opensAndCloses,
                                             Dictionary<DateTime, decimal> range1, Dictionary<DateTime, decimal> range2) {
            this.opensAndCloses = opensAndCloses;
            this.range1 = range1;
            this.range2 = range2;
        }

        /// <summary>
        /// Calculate as a percentage the amount of time that the price data stays within the two ranges.
        /// </summary>
        /// <returns></returns>
        public decimal Calculate() {
            decimal count = this.opensAndCloses.Count(
                q =>
                AreOpenAndCloseWithinRange(
                    q.Item2, q.Item3, 
                    range1.First(r => r.Key == q.Item1).Value,
                    range2.First(r => r.Key == q.Item1).Value));
            return (count / this.opensAndCloses.Count()) * 100;
        }

        private static bool AreOpenAndCloseWithinRange(decimal open, decimal close, decimal lowerRange,
                                                       decimal upperRange) {
            return open.IsBetween(lowerRange, upperRange) && close.IsBetween(lowerRange, upperRange);
        }
    }
}
