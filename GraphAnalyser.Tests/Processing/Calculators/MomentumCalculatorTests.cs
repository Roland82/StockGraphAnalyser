

namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;
    using TestUtilities;

    public class MomentumCalculatorTests
    {
        [Test]
        public void NotEnoughPriceDataToCalculate() {
            var closingPrices = GraphPlottingUtilities.CreateGraph(
                DateTime.MinValue,
                new[] { 98m, 99m, 100m, 100m }
            );  
            
            Assert.Throws<ArgumentException>(() => new MomentumCalculator(closingPrices, 4));
        }

        [Test]
        public void PriceDataMinimumSatisfied()
        {
            const int period = 3;

            var closingPrices = GraphPlottingUtilities.CreateGraph(
                DateTime.MinValue,
                new[] { 100m, 100m, 100m, 100m }
            );

            var expectedMomentum = GraphPlottingUtilities.CreateGraph(
                DateTime.MinValue.AddDays(period),
                new[] { 100m }
            );

            var calculator = new MomentumCalculator(closingPrices, period);
            Assert.AreEqual(expectedMomentum, calculator.Calculate().Result);
        }

        [Test]
        public void CalculateTest() {
            const int period = 10;

            var closingPrices = GraphPlottingUtilities.CreateGraph(
                DateTime.MinValue,
                new[] { 98m, 99m, 100m, 100m, 101m, 101m, 101m, 101m, 101m, 101m, 101m, 103m, 105m}
            );

            var expectedMomentum = GraphPlottingUtilities.CreateGraph(
                DateTime.MinValue.AddDays(10), 
                new[] {103.1m, 104m, 105m}
            );

            var calculator = new MomentumCalculator(closingPrices, period);
            Assert.AreEqual(expectedMomentum, calculator.Calculate().Result);
        }
    }
}
