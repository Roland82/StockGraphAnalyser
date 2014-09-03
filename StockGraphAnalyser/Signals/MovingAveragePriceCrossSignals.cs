

namespace StockGraphAnalyser.Signals
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Processing;

    public class MovingAveragePriceCrossSignals : IGenerateSignals
    {
        private readonly IEnumerable<DataPoints> dataPoints;

        public MovingAveragePriceCrossSignals(IEnumerable<DataPoints> dataPoints) {
            this.dataPoints = dataPoints;
        }

        public Dictionary<DateTime, SignalType> GenerateSignals() {
            this.dataPoints.ForEachGroup(2, group =>
                {
                    if (ele )
                });
        }


    }
}
