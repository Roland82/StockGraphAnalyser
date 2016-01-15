

namespace GraphAnalyser.Tests.Signals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Signals;
    using StockGraphAnalyser.Signals.TradingStrategies;

    [TestFixture]
    public class SignalGeneratorTests
    {

        [Test]
        public void OneStrategyUsedTest()
        {
            var strategy = new Mock<AbstractTradingStrategy>();
            StrategyApplicabilitySetup(strategy, DateTime.Today, DateTime.Today.AddDays(4), true);
            StrategySignalSetup(strategy, SignalType.Buy, DateTime.Today);
            StrategySignalSetup(strategy, SignalType.TakeProfits, DateTime.Today.AddDays(4));

            var signalGenerator = new SignalGenerator(this.SetupDatapoints(), new[] { strategy.Object });
            var signals = signalGenerator.Generate();
            Assert.AreEqual(new[] { SignalType.Buy, SignalType.TakeProfits, }, signals.Select(s => s.SignalType).ToArray());
        }

        [Test]
        public void TwoStrategiesUsedTest()
        {
            var strategy1 = new Mock<AbstractTradingStrategy>();
            var strategy2 = new Mock<AbstractTradingStrategy>();

            StrategyApplicabilitySetup(strategy1, DateTime.Today, DateTime.Today.AddDays(4), true);
            StrategyApplicabilitySetup(strategy1, DateTime.Today.AddDays(5), DateTime.Today.AddDays(7), false);
            StrategyApplicabilitySetup(strategy2, DateTime.Today, DateTime.Today.AddDays(4), false);
            StrategyApplicabilitySetup(strategy2, DateTime.Today.AddDays(5), DateTime.Today.AddDays(7), true);
            
            StrategySignalSetup(strategy1, SignalType.Buy, DateTime.Today);
            StrategySignalSetup(strategy1, SignalType.TakeProfits, DateTime.Today.AddDays(4));
            StrategySignalSetup(strategy2, SignalType.Sell, DateTime.Today.AddDays(6));

            var signalGenerator = new SignalGenerator(this.SetupDatapoints(), new[] { strategy1.Object, strategy2.Object });
            var signals = signalGenerator.Generate();
            Assert.AreEqual(new[] { SignalType.Buy, SignalType.TakeProfits, SignalType.Sell }, signals.Select(s => s.SignalType).ToArray());
        }

        /// <summary>
        /// There's a chance that a strategy might not be applicable anymore even though a position created by the strategy is still open.
        /// We need to ensure that strategy continues to be used until it closes the position out
        /// </summary>
        [Test]
        public void StrategyUsedUntilPositionIsClosedTest()
        {
            var strategy = new Mock<AbstractTradingStrategy>();
            StrategyApplicabilitySetup(strategy, DateTime.Today, DateTime.Today.AddDays(1), true);
            StrategyApplicabilitySetup(strategy, DateTime.Today.AddDays(2), DateTime.Today.AddDays(7), false);
            StrategySignalSetup(strategy, SignalType.Buy, DateTime.Today);
            StrategySignalSetup(strategy, SignalType.TakeProfits, DateTime.Today.AddDays(4));

            // The below signal should not be returned as the stategy should no longer be applicable
            StrategySignalSetup(strategy, SignalType.Sell, DateTime.Today.AddDays(6));

            var signalGenerator = new SignalGenerator(this.SetupDatapoints(), new[] { strategy.Object });
            var signals = signalGenerator.Generate();
            Assert.AreEqual(new[] { SignalType.Buy, SignalType.TakeProfits, }, signals.Select(s => s.SignalType).ToArray());
        }

        private IEnumerable<DataPoints> SetupDatapoints()
        {
            var quotes = new[]
                {
                    Quote.Create("TEST.L", DateTime.Today, 0, 44.52m, 44.53m, 43.98m, 1000),
                    Quote.Create("TEST.L", DateTime.Today.AddDays(1), 0, 44.65m, 44.93m, 44.36m, 1000),
                    Quote.Create("TEST.L", DateTime.Today.AddDays(2), 0, 45.22m, 45.39m, 44.70m, 1000),
                    Quote.Create("TEST.L", DateTime.Today.AddDays(3), 0, 45.45m, 45.70m, 45.13m, 1000),
                    Quote.Create("TEST.L", DateTime.Today.AddDays(4), 0, 45.49m, 45.63m, 44.89m, 1000),
                    Quote.Create("TEST.L", DateTime.Today.AddDays(5), 0, 45.49m, 45.63m, 44.89m, 1000),
                    Quote.Create("TEST.L", DateTime.Today.AddDays(6), 0, 45.49m, 45.63m, 44.89m, 1000),
                    Quote.Create("TEST.L", DateTime.Today.AddDays(7), 0, 45.49m, 45.63m, 44.89m, 1000)
                };
            return quotes.Select(DataPoints.CreateFromQuote);
        }

        private static void StrategySignalSetup(Mock<AbstractTradingStrategy> strategy, SignalType returnsSignalType, DateTime onDay) {
            strategy.Setup(m => m.ActionToTake(onDay, null)).Returns(new Signal { SignalType = returnsSignalType, Date = onDay });       
        }

        private static void StrategyApplicabilitySetup(Mock<AbstractTradingStrategy> strategy, DateTime startDay, DateTime endDay, bool appliesOrNot)
        {
            strategy.Setup(m => m.IsApplicableTo(It.IsInRange(startDay, endDay, Range.Inclusive))).Returns(appliesOrNot);
        }
    }
}
