

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ResistanceAndSupportPredictabilityCalculator
    {
        public enum Type { Resistance, Support }

        private readonly Type calulatorType ; 
        private readonly IEnumerable<Tuple<DateTime, decimal, decimal>> opensAndCloses;
        private readonly Dictionary<DateTime, decimal> range;

        public ResistanceAndSupportPredictabilityCalculator(Type calulatorType, IEnumerable<Tuple<DateTime, decimal, decimal>> opensAndCloses, Dictionary<DateTime, decimal> range) {
            this.calulatorType = calulatorType;
            this.opensAndCloses = opensAndCloses;
            this.range = range;
        }

        /// <summary>
        /// Calculate as a percentage the amount of time that the price data stays within the two ranges.
        /// </summary>
        /// <returns></returns>
        public decimal Calculate()
        {
            var count = this.calulatorType == Type.Resistance ? 
                this.opensAndCloses.Count(q => this.range[q.Item1] >= q.Item2 && this.range[q.Item1] >= q.Item3) : 
                this.opensAndCloses.Count(q => this.range[q.Item1] <= q.Item2 && this.range[q.Item1] <= q.Item3);

            return (count / (decimal)this.opensAndCloses.Count()) * 100;
        }
    }
}
