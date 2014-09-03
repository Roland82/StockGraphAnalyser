

namespace StockGraphAnalyser.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Processing;

    public class MovingAveragePriceCrossSignals : IGenerateSignals
    {
        private readonly IEnumerable<DataPoints> dataPoints;

        public MovingAveragePriceCrossSignals(IEnumerable<DataPoints> dataPoints) {
            this.dataPoints = dataPoints;
        }

        public IEnumerable<Signal> GenerateSignals()
        {
            return this.dataPoints.ForEachGroup(2, group =>
                {
                    if (group.ElementAt(0).Close >= group.ElementAt(0).MovingAverageFiftyDay && group.ElementAt(1).Close < group.ElementAt(1).MovingAverageFiftyDay)
                    {
                        return Signal.Create(group.ElementAt(1).Symbol, group.ElementAt(1).Date, SignalType.Sell, group.ElementAt(1).Close);
                    } 

                    if (group.ElementAt(0).Close <= group.ElementAt(0).MovingAverageFiftyDay && group.ElementAt(1).Close > group.ElementAt(1).MovingAverageFiftyDay)
                    {
                        return Signal.Create(group.ElementAt(1).Symbol, group.ElementAt(1).Date, SignalType.Buy, group.ElementAt(1).Close);
                    }

                    return null;
                }).Where(e => e != null);
        }
    }
}
