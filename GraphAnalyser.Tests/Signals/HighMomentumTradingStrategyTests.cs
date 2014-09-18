

namespace GraphAnalyser.Tests.Signals
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Signals;

    public class HighMomentumTradingStrategyTests
    {
        [TestCase(100, 200, 300, 400, 500, true)]
        [TestCase(100, 100.5, 101, 101.5, 102.1, true)]
        [TestCase(100, 100.5, 101, 101.5, 102.0, false)]
        [TestCase(100, 100.5, 101, 100.4, 102, false)]
        public void IsApplicableToTests(decimal maDay1, decimal maDay2, decimal maDay3, decimal maDay4, decimal maDay5, bool expResult)  {
            var datapoints = new List<DataPoints>{
                                                      new DataPoints() { Date = DateTime.Today.AddDays(0), MovingAverageFiftyDay = maDay1 },
                                                      new DataPoints() { Date = DateTime.Today.AddDays(1), MovingAverageFiftyDay = maDay2 },
                                                      new DataPoints() { Date = DateTime.Today.AddDays(2), MovingAverageFiftyDay = maDay3 },
                                                      new DataPoints() { Date = DateTime.Today.AddDays(3), MovingAverageFiftyDay = maDay4 },
                                                      new DataPoints() { Date = DateTime.Today.AddDays(4), MovingAverageFiftyDay = maDay5 }
                                                      };

            var strategy = new HighMomentumTradingStrategy(datapoints);
            Assert.AreEqual(expResult, strategy.IsApplicableTo(DateTime.Today.AddDays(4)));
        }
    }
}
