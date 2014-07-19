
namespace StockGraphAnalyser.Processing.Candlestick
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Types;

    public class BullishKickerPatternRecogniser : IDetectPattern
    {
        private readonly IEnumerable<Quote> quotes;

        public BullishKickerPatternRecogniser(IEnumerable<Quote> quotes) {
            this.quotes = quotes;
        }

        public DateTime? LatestOccurence() {
            var count = quotes.Count() - 4;
            
            for (var i = 0; i <= count; i++)
            {
                var sample = quotes.Skip(i).Take(4);
                if (sample.Take(3).All(q => q.OpenToClosePercentageMovement < 0)
                    && sample.ElementAt(3).Open < sample.ElementAt(2).High
                    && sample.ElementAt(3).OpenToClosePercentageMovement > 0)
                {
                    return sample.ElementAt(4).Date;
                }
                
            }

            return null;
        }
    }
}
