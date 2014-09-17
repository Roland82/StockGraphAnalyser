

namespace GraphAnalyser.Tests.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StockGraphAnalyser.Domain;
    using TestUtilities;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing;

    [TestFixture]
    public class LinqExtensionsTests
    {
        [TestCase(1, 0)]
        [TestCase(10, 4)]
        [TestCase(200, null)]
        public void IndexOfTest(int numberToFind, int? expectedResult) {
            var testCollection = new[] { 1, 2, 4, 8, 10 };
            var result = testCollection.IndexOf(i => i == numberToFind);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SubsetBetweenTest(){
            var graph = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[]{1m, 2m, 3m, 4m});
            var expectedResult = new Dictionary<DateTime, decimal>{{DateTime.Today.AddDays(1), 2}, {DateTime.Today.AddDays(2), 3}};
            var result = graph.SubsetBetween(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SubsetBetweenTestPartialMatch() {
            var monday = new DateTime(2014, 9, 1);
            var graph = GraphPlottingUtilities.CreateGraph(monday, new[] { 1m, 2m, 3m, 4m });
            var expectedResult = new Dictionary<DateTime, decimal> { { monday.AddDays(1), 2 }, { monday.AddDays(2), 3 }, { monday.AddDays(3), 4 } };
            var result = graph.SubsetBetween(monday.AddDays(1), monday.AddDays(10));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ForEachGroupTest() {
            var numbers = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var executedGroups = new List<IEnumerable<int>>();
            numbers.ForEachGroup(9, executedGroups.Add);
            Assert.AreEqual(2, executedGroups.Count);
            Assert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, executedGroups.ElementAt(0));
            Assert.AreEqual(new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 }, executedGroups.ElementAt(1));
        }

        [Test]
        public void MapNewDatapointTest() {
            var quotes = new[]
                {
                    Quote.Create("TST.L", DateTime.Today.AddDays(0), 1, 2, 3, 4, 5),
                    Quote.Create("TST.L", DateTime.Today.AddDays(1), 1, 2, 3, 4, 5),
                    Quote.Create("TST.L", DateTime.Today.AddDays(2), 1, 2, 3, 4, 5),
                    Quote.Create("TST.L", DateTime.Today.AddDays(3), 1, 2, 3, 4, 5),
                    Quote.Create("TST.L", DateTime.Today.AddDays(4), 1, 2, 3, 4, 5),
                    Quote.Create("TST.L", DateTime.Today.AddDays(5), 1, 2, 3, 4, 5),
                };

            var datapoints = quotes.Select(DataPoints.CreateFromQuote);

            var newDataToMap = new Dictionary<DateTime, decimal>
                {
                    {DateTime.Today.AddDays(0), 10},
                    {DateTime.Today.AddDays(1), 11},
                    {DateTime.Today.AddDays(2), 12},
                    {DateTime.Today.AddDays(3), 13},
                    {DateTime.Today.AddDays(4), 14},
                    {DateTime.Today.AddDays(8), 19}
                };

            var newDatapoints = datapoints.MapNewDataPoint(newDataToMap, (d, n) => d.MovingAverageFiftyDay = n);
            Assert.True(newDatapoints.Any(d => d.Date == DateTime.Today.AddDays(0) && d.MovingAverageFiftyDay == 10));
            Assert.True(newDatapoints.Any(d => d.Date == DateTime.Today.AddDays(1) && d.MovingAverageFiftyDay == 11));
            Assert.True(newDatapoints.Any(d => d.Date == DateTime.Today.AddDays(2) && d.MovingAverageFiftyDay == 12));
            Assert.True(newDatapoints.Any(d => d.Date == DateTime.Today.AddDays(3) && d.MovingAverageFiftyDay == 13));
            Assert.True(newDatapoints.Any(d => d.Date == DateTime.Today.AddDays(4) && d.MovingAverageFiftyDay == 14));
            Assert.True(newDatapoints.Any(d => d.Date == DateTime.Today.AddDays(5) && !d.MovingAverageFiftyDay.HasValue));
        }
    }
}
