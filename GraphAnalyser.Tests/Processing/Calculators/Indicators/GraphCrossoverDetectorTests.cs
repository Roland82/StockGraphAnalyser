
namespace GraphAnalyser.Tests.Processing.Calculators.Indicators
{
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators.Indicators;
    using System;
    using TestUtilities;

    public class GraphCrossoverDetectorTests
    {
        [Test]
        public void Graph1CrossDownTest()
        {
            var ma50Prices = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[] { 4m, 3m, 2m, 1m });
            var ma200Prices = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[]{1m, 2m, 3m, 4m});

            var crossoverDetector = new GraphCrossoverDetector(ma50Prices, ma200Prices);
            var result = crossoverDetector.Calculate();
            Assert.AreEqual(DateTime.Today.AddDays(2), result.Value);
        }

        [Test]
        public void Graph1CrossUpTest()
        {
            var ma50Prices = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[] { 1m, 2m, 3m, 4m });
            var ma200Prices = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[] { 4m, 3m, 2m, 1m });
            
            var crossoverDetector = new GraphCrossoverDetector(ma50Prices, ma200Prices);
            var result = crossoverDetector.Calculate();
            Assert.AreEqual(DateTime.Today.AddDays(2), result.Value);
        }

        [Test]
        public void TotalCrossoverTest()
        {
            var ma200Prices = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[] { 1m, 2m, 3m, 4m });
            var ma50Prices = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[] { 4m, 3m, 2m, 1m });

            var crossoverDetector = new GraphCrossoverDetector(ma50Prices, ma200Prices);
            var result = crossoverDetector.Calculate();
            Assert.AreEqual(DateTime.Today.AddDays(2), result.Value);
        }

        [Test]
        public void TouchingTest()
        {
            var ma200Prices = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[] { 1m, 2m, 3m, 3m });
            var ma50Prices = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[] { 4m, 3m, 3m, 2m });

            var crossoverDetector = new GraphCrossoverDetector(ma50Prices, ma200Prices);
            var result = crossoverDetector.Calculate();
            Assert.AreEqual(DateTime.Today.AddDays(2), result.Value);
        }

        [Test]
        public void NoCrossoverTest()
        {
            var ma200Prices = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[] { 5m, 6m, 4m, 4m });
            var ma50Prices = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[] { 4m, 3m, 3m, 2m });

            var crossoverDetector = new GraphCrossoverDetector(ma50Prices, ma200Prices);
            Assert.Null(crossoverDetector.Calculate());
        }
    }
}
