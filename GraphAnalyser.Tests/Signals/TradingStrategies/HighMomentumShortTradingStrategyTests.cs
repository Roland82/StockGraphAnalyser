namespace GraphAnalyser.Tests.Signals.TradingStrategies
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Signals.TradingStrategies;

    public class HighMomentumShortTradingStrategyTests
    {
        // Shorting Criteria met
        [TestCase(100, 90, 80, 70, 60, 6, true)]
        [TestCase(100, 99.5, 99, 98.5, 98, 6, true)]
        // Minimum percentage decrease not met
        [TestCase(100, 99.5, 99, 98.5, 98.1, 6, false)]
        [TestCase(100, 99.8, 99.9, 99.5, 99.1, 6, false)]
        // Tests for where there isnt enough data to determine applicability
        [TestCase(100, 100.5, 101, 101.5, 102.1, 5, false)]
        [TestCase(100, 100.5, 101, 101.5, 102.1, 4, false)]
        [TestCase(100, 100.5, 101, 101.5, 102.1, 3, false)]
        public void IsApplicableToTest(decimal maDay1, decimal maDay2, decimal maDay3, decimal maDay4, decimal maDay5, int dayOffset, bool expResult)
        {
            var datapoints = new List<DataPoints>{
                                                      new DataPoints() { Date = DateTime.Today.AddDays(0) },
                                                      new DataPoints() { Date = DateTime.Today.AddDays(1) },
                                                      new DataPoints() { Date = DateTime.Today.AddDays(2), MovingAverageFiftyDay = maDay1 },
                                                      new DataPoints() { Date = DateTime.Today.AddDays(3), MovingAverageFiftyDay = maDay2 },
                                                      new DataPoints() { Date = DateTime.Today.AddDays(4), MovingAverageFiftyDay = maDay3 },
                                                      new DataPoints() { Date = DateTime.Today.AddDays(5), MovingAverageFiftyDay = maDay4 },
                                                      new DataPoints() { Date = DateTime.Today.AddDays(6), MovingAverageFiftyDay = maDay5 }
                                                 };

            var strategy = new HighMomentumShortTradingStrategy(datapoints);
            Assert.AreEqual(expResult, strategy.IsApplicableTo(DateTime.Today.AddDays(dayOffset)));
        }
    }
}
