
namespace StockGraphAnalyser.Processing.Candlestick
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    public class EngulfingPatterRecogniser : IDetectPattern
    {
        public enum Type
        {
            Bearish,
            Bullish
        };

        private readonly IEnumerable<DataPoints> quotes;
        private readonly Type engulfingType;
        private readonly int patternType;

        public EngulfingPatterRecogniser(IEnumerable<DataPoints> quotes, Type engulfingType) {
            this.quotes = quotes;
            this.engulfingType = engulfingType;
            patternType = engulfingType == Type.Bullish ? 1 : 2;
        }

        public int PatternType { get { return patternType; } }

        public IEnumerable<DateTime> FindOccurences() {
            var count = this.quotes.Count();

            for (var i = count - 1; i >= 1; i--)
            {
                var previousDaysQuote = quotes.ElementAt(i - 1);
                var currentDaysQuote = quotes.ElementAt(i);


                if (!previousDaysQuote.Open.IsBetween(currentDaysQuote.Open, currentDaysQuote.Close) ||
                    !previousDaysQuote.Close.IsBetween(currentDaysQuote.Open, currentDaysQuote.Close))
                {
                    continue;
                }

                if (this.IsEngulfingOfCorrectType(previousDaysQuote, currentDaysQuote))
                {
                    yield return currentDaysQuote.Date;
                }
            }
        }

        private bool IsEngulfingOfCorrectType(DataPoints previousDayQuote, DataPoints currentDayQuote) {
            if (previousDayQuote.PercentageChange > 0 && currentDayQuote.PercentageChange < 0)
            {
                return this.engulfingType == Type.Bearish;
            }

            return this.engulfingType == Type.Bullish;
        }
    }
}
