

namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;
    using TestUtilities;

    public class RangePredictabilityCalculatorTests
    {
        private RangePredictabilityCalculator calculator;
        private DateTime monday = new DateTime(2014, 8, 4);

        [Test]
        public void AllDataWithinRange() {
            var opensAndCloses = new List<Tuple<DateTime, decimal, decimal>>
                {
                    new Tuple<DateTime, decimal, decimal>(monday, 2, 3),
                    new Tuple<DateTime, decimal, decimal>(monday.AddDays(1), 3, 4),
                    new Tuple<DateTime, decimal, decimal>(this.monday.AddDays(2), 4, 3),
                    new Tuple<DateTime, decimal, decimal>(monday.AddDays(3), 3, 2)
                };

            var range1 = GraphPlottingUtilities.CreateGraph(monday, new[] { 2m, 3m, 3m, 2m});
            var range2 = GraphPlottingUtilities.CreateGraph(monday, new[] { 3m, 4m, 4m, 3m });
            this.calculator = new RangePredictabilityCalculator(opensAndCloses, range1, range2);
            Assert.AreEqual(100m, this.calculator.Calculate());
        }

        [Test]
        public void FiftyPercentDataWithinRange()
        {
            var opensAndCloses = new List<Tuple<DateTime, decimal, decimal>>
                {
                    new Tuple<DateTime, decimal, decimal>(monday, 2, 4),
                    new Tuple<DateTime, decimal, decimal>(monday.AddDays(1), 1, 4),
                    new Tuple<DateTime, decimal, decimal>(this.monday.AddDays(2), 4, 3),
                    new Tuple<DateTime, decimal, decimal>(monday.AddDays(3), 3, 2)
                };

            var range1 = GraphPlottingUtilities.CreateGraph(monday, new[] { 3m, 2m, 3m, 2m });
            var range2 = GraphPlottingUtilities.CreateGraph(monday, new[] { 4m, 4m, 4m, 3m });
            this.calculator = new RangePredictabilityCalculator(opensAndCloses, range1, range2);
            Assert.AreEqual(50m, this.calculator.Calculate());
        }
    }
}
