
namespace GraphAnalyser.Tests.Processing
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing;

    [TestFixture]
    public class UtilitiesTests
    {
        [TestCase(new[] { 1, 2, 3 }, new[] { 2 }, new[] { 2, 2 })]
        [TestCase(new[] { 2 }, new[] { 1, 2, 3 }, new[] { 2, 2 })]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 3, 4, 5, 6 }, new[] { 3, 4 })]
        public void CalculateSharedDateRangeTestOne(int[] range1, int[] range2, int[] expectedOutputRange)
        {
            var dates1 = range1.Select(i => DateTime.Today.AddDays(i));
            var dates2 = range2.Select(i => DateTime.Today.AddDays(i));
            var actualOutput = Utilities.CalculateSharedDateRange(dates1, dates2);
            var expectedOutput = new Tuple<DateTime, DateTime>(DateTime.Today.AddDays(expectedOutputRange[0]), DateTime.Today.AddDays(expectedOutputRange[1]));
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [Test]
        public void CalculateSharedDateRangeNoDatesShared()
        {
            var dates1 = new[] { 1, 2, 3, 4 }.Select(i => DateTime.Today.AddDays(i));
            var dates2 = new[] { 6, 5, 7 }.Select(i => DateTime.Today.AddDays(i));
            var actualOutput = Utilities.CalculateSharedDateRange(dates1, dates2);
            Assert.Null(actualOutput);
        }
    }
}
