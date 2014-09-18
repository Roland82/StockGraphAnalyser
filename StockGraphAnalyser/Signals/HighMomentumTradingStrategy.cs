
namespace StockGraphAnalyser.Signals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Processing;

    public class HighMomentumTradingStrategy : AbstractTradingStrategy
    {
        private readonly IEnumerable<DataPoints> dataPoints;

        public HighMomentumTradingStrategy(IEnumerable<DataPoints> dataPoints)
        {
            this.dataPoints = dataPoints;
        }

        public override bool IsApplicableTo(DateTime date)
        {
            var numberOfDataPoints = dataPoints.Count();
            var latestDatapoints = dataPoints
               .OrderBy(d => d.Date)
               .Skip(numberOfDataPoints - 5)
               .Take(5).ToList();

            var necessaryAverageValueIncrease = (latestDatapoints.First().MovingAverageFiftyDay / 100) / 2;
            var average50DayMaRise = latestDatapoints.ForEachGroup(2, e => MathExtras.PercentageDifferenceBetween(e.ElementAt(0).MovingAverageFiftyDay.Value, e.ElementAt(1).MovingAverageFiftyDay.Value)).Average();

            return average50DayMaRise >= necessaryAverageValueIncrease;
        }

        public override bool SatisfiesShortCriteria(DateTime date)
        {
            throw new NotImplementedException();
        }

        public override bool SatisfiesLongCriteria(DateTime date)
        {
            throw new NotImplementedException();
        }

        public override bool SatisfiesTakeProfitsFromLongCriteria(DateTime date)
        {
            throw new NotImplementedException();
        }

        public override bool SatisfiesTakeProfitsFromShortCriteria(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
