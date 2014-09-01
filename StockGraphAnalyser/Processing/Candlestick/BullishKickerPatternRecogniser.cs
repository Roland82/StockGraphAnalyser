
namespace StockGraphAnalyser.Processing.Candlestick
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    public class BullishKickerPatternRecogniser : IDetectPattern
    {
        private readonly IEnumerable<DataPoints> datapoints;

        public BullishKickerPatternRecogniser(IEnumerable<DataPoints> datapoints) {
            this.datapoints = datapoints;
        }

        public int PatternType { get { return 3; } }

        public IEnumerable<DateTime> FindOccurences() {
            var count = this.datapoints.Count() - 4;
            
            for (var i = 0; i <= count; i++)
            {
                var sample = this.datapoints.Skip(i).Take(4);
                if (sample.Take(3).All(q => q.PercentageChange < 0)
                    && sample.ElementAt(3).Open < sample.ElementAt(2).High
                    && sample.ElementAt(3).PercentageChange > 0)
                {
                    yield return sample.ElementAt(4).Date;
                }            
            }
        }
    }
}
