

namespace GraphAnalyser.Tests.Processing
{
    using System;
    using System.Collections.Generic;
    using TestUtilities;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing;

    [TestFixture]
    public class LinqExtensionsTests
    {
        [Test]
        public void SubsetBetweenTest(){
            var graph = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[]{1m, 2m, 3m, 4m});
            var expectedResult = new Dictionary<DateTime, decimal>{{DateTime.Today.AddDays(1), 2}, {DateTime.Today.AddDays(2), 3}};
            var result = graph.SubsetBetween(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SubsetBetweenTestPartialMatch()
        {
            var graph = GraphPlottingUtilities.CreateGraph(DateTime.Today, new[] { 1m, 2m, 3m, 4m });
            var expectedResult = new Dictionary<DateTime, decimal> { { DateTime.Today.AddDays(1), 2 }, { DateTime.Today.AddDays(2), 3 }, { DateTime.Today.AddDays(3), 4 } };
            var result = graph.SubsetBetween(DateTime.Today.AddDays(1), DateTime.Today.AddDays(10));
            Assert.AreEqual(expectedResult, result);
        }
    }
}
