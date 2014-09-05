
namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;
    using TestUtilities;

    [TestFixture]
    public class DifferenceCalculatorTests
    {
        private readonly DateTime monday = new DateTime(2014, 8 , 4);

        [Test]
        public void HistogramCalculationTest()
        {
            var slowMa = GraphPlottingUtilities.CreateGraph(monday, new[] { 2m, 3m, 4m, 5m, 5m, 1m });
            var fastMa = GraphPlottingUtilities.CreateGraph(monday, new[] { 1m, 3m, 5m, 8m, 1m, 1m });
            var graphPlotter = new DifferenceCalculator(fastMa, slowMa);
            var expectedReturnDictionary = GraphPlottingUtilities.CreateGraph(monday,
                                                                              new[] { -1m, 0m, 1m, 3m, -4m, 0m});
            Assert.AreEqual(graphPlotter.CalculateAsync().Result, expectedReturnDictionary);
        }

        [Test]
        public void HistogramPartialCalculationTest()
        {
            var slowMa = GraphPlottingUtilities.CreateGraph(monday, new[] { 2m, 3m, 4m, 5m, 5m, 1m });
            var fastMa = GraphPlottingUtilities.CreateGraph(monday, new[] { 1m, 3m, 5m, 8m, 1m, 1m });
            var graphPlotter = new DifferenceCalculator(fastMa, slowMa);
            var expectedReturnDictionary = GraphPlottingUtilities.CreateGraph(monday.AddDays(7),
                                                                              new[] { 0m });
            Assert.AreEqual(graphPlotter.CalculateAsync(monday.AddDays(7)).Result, expectedReturnDictionary);
        }

        [Test]
        public void HistogramPartialCalculationNoDataTest()
        {
            var slowMa = GraphPlottingUtilities.CreateGraph(monday, new[] { 2m, 3m, 4m, 5m, 5m, 1m });
            var fastMa = GraphPlottingUtilities.CreateGraph(monday, new[] { 1m, 3m, 5m, 8m, 1m, 1m });
            var graphPlotter = new DifferenceCalculator(fastMa, slowMa);
            var expectedReturnDictionary = GraphPlottingUtilities.CreateGraph(monday.AddDays(8), new decimal[] { });
            Assert.AreEqual(graphPlotter.CalculateAsync(monday.AddDays(8)).Result, expectedReturnDictionary);
        }
    }
}
