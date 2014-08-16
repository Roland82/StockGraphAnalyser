

namespace GraphAnalyser.Tests.Processing.TradingSignalDetectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Processing.Calculators.TradingSignalDetectors;

    public class OverboughtInDowntrendSignalTests
    {
        // TODO: Implement this test when class finished
        public void TestOne() {
            var closes = new[] {5m, 6m, 7m, 8m, 9m};
            var upperBollinger = new[] {7m, 7m, 7m, 8m, 9m};
            var ma50Day = new[] { 6m, 5m, 4m, 3m, 2m };
            var signalDetector = new OverboughtInDowntrendSignal(this.CreateDataPoints(closes, upperBollinger, ma50Day));
            Assert.AreEqual(DateTime.Today.AddDays(2), signalDetector.FindLatestSignal());
            
        }

        private IEnumerable<DataPoints> CreateDataPoints(IEnumerable<decimal> closes, IList<decimal> upperBollinger, decimal[] ma50Day) {
            return closes.Select((t, i) => new DataPoints()
                {
                    Date = DateTime.Today.AddDays(i),
                    Close = t,
                    UpperBollingerBandTwoDeviation = upperBollinger[i],
                    MovingAverageFiftyDay = ma50Day[i]
                });
        }
    }
}
