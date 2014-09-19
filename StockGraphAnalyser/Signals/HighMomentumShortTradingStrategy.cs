

namespace StockGraphAnalyser.Signals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Processing;

    public class HighMomentumShortTradingStrategy : AbstractTradingStrategy
    {
        public HighMomentumShortTradingStrategy(IEnumerable<DataPoints> dataPoints) : base(dataPoints) {
        }

        public override bool IsApplicableTo(DateTime date) {
            var isThereEnoughData = this.DataPoints.Count(d => d.Date <= date && d.MovingAverageFiftyDay.HasValue) >= 5;
            if (isThereEnoughData)
            {
                var latestDatapoints = this.DataPoints.OrderBy(d => d.Date).Skip(this.DataPoints.Count() - 5).Take(5).ToList();
                var necessaryAverageValueChange = (latestDatapoints.First().MovingAverageFiftyDay/100)/2;
                var average50DayMaChange = latestDatapoints.ForEachGroup(2, e =>
                                                                            MathExtras.PercentageDifferenceBetween(
                                                                                e.ElementAt(0)
                                                                                 .MovingAverageFiftyDay.Value,
                                                                                e.ElementAt(1)
                                                                                 .MovingAverageFiftyDay.Value))
                                                           .Average();

                return average50DayMaChange <= -necessaryAverageValueChange;
            }

            return false;
        }

        public override bool SatisfiesCriteria(DateTime date) {
            throw new NotImplementedException();
        }

        public override bool SatisfiesTakeProfitsCriteria(DateTime date) {
            throw new NotImplementedException();
        }
    }
}
