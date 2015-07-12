

namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using GraphAnalyser.Tests.TestUtilities;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;

    [TestFixture]
    public class ResistanceAndSupportPredictabilityCalculatorTests
    {
        private ResistanceAndSupportPredictabilityCalculator calculator;
        private DateTime monday = new DateTime(2014, 8, 4);

        [Test]
        public void SupportAllWithinRange()
        {
            var opensAndCloses = new List<Tuple<DateTime, decimal, decimal>>
                {
                    new Tuple<DateTime, decimal, decimal>(monday, 2, 3),
                    new Tuple<DateTime, decimal, decimal>(monday.AddDays(1), 3, 4),
                    new Tuple<DateTime, decimal, decimal>(this.monday.AddDays(2), 4, 3),
                    new Tuple<DateTime, decimal, decimal>(monday.AddDays(3), 3, 2)
                };

            var range = GraphPlottingUtilities.CreateGraph(monday, new[] { 1m, 2m, 2m, 2m });
            this.calculator = new ResistanceAndSupportPredictabilityCalculator(ResistanceAndSupportPredictabilityCalculator.Type.Support, opensAndCloses, range);
            Assert.AreEqual(100m, this.calculator.Calculate());
        }

        [Test]
        public void SupportParitallyWithinRange()
        {
            var opensAndCloses = new List<Tuple<DateTime, decimal, decimal>>
                {
                    new Tuple<DateTime, decimal, decimal>(monday, 2, 3),
                    new Tuple<DateTime, decimal, decimal>(monday.AddDays(1), 3, 4),
                    new Tuple<DateTime, decimal, decimal>(this.monday.AddDays(2), 4, 3),
                    new Tuple<DateTime, decimal, decimal>(monday.AddDays(3), 3, 2)
                };

            var range = GraphPlottingUtilities.CreateGraph(monday, new[] { 2m, 9m, 3m, 2m });
            this.calculator = new ResistanceAndSupportPredictabilityCalculator(ResistanceAndSupportPredictabilityCalculator.Type.Support, opensAndCloses, range);
            Assert.AreEqual(75m, this.calculator.Calculate());
        }

        [Test]
        public void ResistanceParitallyWithinRange()
        {
            var opensAndCloses = new List<Tuple<DateTime, decimal, decimal>>
                {
                    new Tuple<DateTime, decimal, decimal>(monday, 9, 10),
                    new Tuple<DateTime, decimal, decimal>(monday.AddDays(1), 10, 10),
                    new Tuple<DateTime, decimal, decimal>(this.monday.AddDays(2), 10, 11),
                    new Tuple<DateTime, decimal, decimal>(monday.AddDays(3), 10, 9)
                };

            var range = GraphPlottingUtilities.CreateGraph(monday, new[] { 10m, 10m, 10m, 11m });
            this.calculator = new ResistanceAndSupportPredictabilityCalculator(ResistanceAndSupportPredictabilityCalculator.Type.Resistance, opensAndCloses, range);
            Assert.AreEqual(75m, this.calculator.Calculate());
        }
    }
}
