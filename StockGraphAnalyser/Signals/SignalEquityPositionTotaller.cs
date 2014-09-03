
namespace StockGraphAnalyser.Signals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StockGraphAnalyser.Domain;

    /// <summary>
    /// A class that will take a list of trading signals and work out the profit/loss as trade signals are acted on.
    /// </summary>
    public class SignalEquityPositionTotaller
    {
        private readonly IEnumerable<Signal> signals;
        private readonly decimal startingEquity;

        public SignalEquityPositionTotaller(IEnumerable<Signal> signals, decimal startingEquity)
        {
            this.signals = signals;
            this.startingEquity = startingEquity;
        }

        public Dictionary<DateTime, decimal> Calculate()
        {
            var returnDictionary = new Dictionary<DateTime, decimal>{{this.signals.ElementAt(0).Date, this.startingEquity}};

            var count = this.signals.Count();

            for (var i = 0; i <= count - 2; i++)
            {
                var startTradePrice = this.signals.ElementAt(i).Price;
                var finalTradePrice = this.signals.ElementAt(i + 1).Price;
                var originalSignal = this.signals.ElementAt(i).SignalType;
                var percentageDifference = (finalTradePrice - startTradePrice) / startTradePrice;
                var percentageDifferenceToEquity = originalSignal == SignalType.Buy && percentageDifference > 0 || originalSignal == SignalType.Sell && percentageDifference < 0
                    ? Math.Abs(percentageDifference) : -Math.Abs(percentageDifference);
                var newEquityValue = returnDictionary.Last().Value + (returnDictionary.Last().Value * percentageDifferenceToEquity);
                returnDictionary.Add(this.signals.ElementAt(i + 1).Date, newEquityValue);
            }


            return returnDictionary;
        }
    }
}
