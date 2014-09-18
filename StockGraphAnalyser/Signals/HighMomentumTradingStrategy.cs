
namespace StockGraphAnalyser.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Processing;

    public class HighMomentumTradingStrategy
    {
        public bool IsApplicableTo(IEnumerable<DataPoints> dataPoints) {
             var numberOfDataPoints = dataPoints.Count();
             var average50DayMaRise = dataPoints
                .OrderBy(d => d.Date)
                .Skip(numberOfDataPoints - 5)
                .Take(5)
                .ForEachGroup(2, e => MathExtras.PercentageDifferenceBetween(e.ElementAt(0).MovingAverageFiftyDay.Value, e.ElementAt(1).MovingAverageFiftyDay.Value))
                .Average();

            return average50DayMaRise > 0.5m;
        }



    }
}
