
namespace StockGraphAnalyser.Processing.Candlestick
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    public class KickerPatternRecogniser : IDetectPattern
    {
        private readonly IEnumerable<DataPoints> datapoints;
        private readonly SentimentType sentimentType;
        private const decimal PercentageChangeRequired = 5;

        public KickerPatternRecogniser(IEnumerable<DataPoints> datapoints, SentimentType sentimentType) {
            this.datapoints = datapoints;
            this.sentimentType = sentimentType;
        }

        public int PatternType { get { return sentimentType == SentimentType.Bullish ? 3 : 4; } }

        public IEnumerable<DateTime> FindOccurences() {
            var count = this.datapoints.Count();
            
            for (var i = 0; i <= count - 2; i++)
            {
                var sample = this.datapoints.Skip(i).Take(2);
                if (this.SampleMatchesCriteria(sample))
                {
                    yield return sample.ElementAt(1).Date;
                }            
            }
        }

        private bool SampleMatchesCriteria(IEnumerable<DataPoints> sample) {
            if (sentimentType == SentimentType.Bullish)
            {
                return sample.ElementAt(1).Open > sample.ElementAt(0).High &&
                       sample.ElementAt(1).PercentageChange > PercentageChangeRequired;
            }

            return sample.ElementAt(1).Open < sample.ElementAt(0).Low &&
                   sample.ElementAt(1).PercentageChange < -PercentageChangeRequired;
        }
    }
}
