
namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;
    using TestUtilities;

    [TestFixture]
    public class MacdHistogramCalculatorTests
    {
        [Test]
        public void HistogramCalculationTest()
        {
            var slowMa = GraphPlottingUtilities.CreateGraph(DateTime.MinValue, new[] { 2m, 3m, 4m, 5m, 5m });
            var fastMa = GraphPlottingUtilities.CreateGraph(DateTime.MinValue, new[] { 1m, 3m, 5m, 8m, 1m });
            var graphPlotter = new MacdHistogramCalculator(fastMa, slowMa);
            var expectedReturnDictionary = GraphPlottingUtilities.CreateGraph(DateTime.MinValue,
                                                                              new[] { 1m, 0m, -1m, -3m, 4m});
            Assert.AreEqual(graphPlotter.CalculateAsync().Result, expectedReturnDictionary);
        }
    }
}
