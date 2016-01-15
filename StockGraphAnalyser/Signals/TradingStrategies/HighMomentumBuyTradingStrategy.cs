
namespace StockGraphAnalyser.Signals.TradingStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Processing;

    public class HighMomentumBuyTradingStrategy : AbstractTradingStrategy
    {
        public HighMomentumBuyTradingStrategy(IEnumerable<DataPoints> dataPoints)
            : base(dataPoints)
        {
        }

        /// <summary>
        /// Determines whether the trading strategy could be applied to the current market conditions on the given date [the specified date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>True if the conditions are correct to use the strategy</returns>
        public override bool IsApplicableTo(DateTime date)
        {
            var isThereEnoughData = this.DataPoints.Count(d => d.Date <= date && d.MovingAverageFiftyDay.HasValue) >= 5;
            

            if (isThereEnoughData)
            {
                var indexOfDatapointToStartAt = this.DataPoints.IndexOf(d => d.Date == date).Value + 1;
                var latestDatapoints = this.DataPoints.OrderBy(d => d.Date).Skip(indexOfDatapointToStartAt - 5).Take(5).ToList();
                var necessaryAverageValueChange = (latestDatapoints.First().MovingAverageFiftyDay/100)/2;
                var average50DayMaChange = latestDatapoints.ForEachGroup(2, e => e.ElementAt(1).MovingAverageFiftyDay.Value - e.ElementAt(0).MovingAverageFiftyDay.Value).Average();

                return average50DayMaChange >= necessaryAverageValueChange;
            }

            return false;
        }


        public override Signal ActionToTake(DateTime date, SignalType? lastSignalType) {
            var datapoint = this.DataPoints.FirstOrDefault(d => d.Date == date);
            if (datapoint == null) { return null; }

            if (SatisfiesCriteriaToBuy(datapoint) && lastSignalType != SignalType.Buy)
            {
                return Signal.Create(datapoint.Symbol, date, SignalType.Buy, datapoint.Close);
            }

            if (this.SatisfiesCriteriaToTakeProfits(datapoint))
            {
                return Signal.Create(datapoint.Symbol, date, SignalType.TakeProfits, datapoint.Close);
            }

            return null;
        }

        private static bool SatisfiesCriteriaToBuy(DataPoints dataPoints) {
            return dataPoints.Close <=
                   dataPoints.LowerBollingerBandTwoDeviation +
                   ((dataPoints.UpperBollingerBandTwoDeviation - dataPoints.LowerBollingerBandTwoDeviation) / 2);
        }

        private bool SatisfiesCriteriaToTakeProfits(DataPoints dataPoints) {
            return dataPoints.Close < dataPoints.LowerBollingerBandTwoDeviation;
        }
    }
}
