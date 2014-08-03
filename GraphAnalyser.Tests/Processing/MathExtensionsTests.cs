namespace GraphAnalyser.Tests.Processing
{
    using NUnit.Framework;
    using StockGraphAnalyser.Processing;

    [TestFixture]
    public class MathExtensionsTests
    {
        [Test]
        public void StandtesterardDeviationTest()
        {
            var dataPoints = new[] { 12m, 15m, 18m, 100m, 39m, 14m };
            Assert.AreEqual(34.28119m, MathExtras.StandardDeviation(dataPoints));
        }

        [TestCase(1, 2, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 0)]
        public void DifferenceTest(decimal number1, decimal number2, int expectedResult)
        {
            Assert.AreEqual(expectedResult, number1.Difference(number2));
        }
    }
}