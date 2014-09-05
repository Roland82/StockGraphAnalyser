

namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Processing.Calculators;

    [TestFixture]
    public class DirectionalMovementCalculatorTests
    {
        private readonly Quote[] quotes =
                {
                    Quote.Create("TST.L", DateTime.Today, 0, 0, 1, 0.5m, 0),
                    Quote.Create("TST.L", DateTime.Today.AddDays(1), 0, 0, 3, 1m, 0),
                    Quote.Create("TST.L", DateTime.Today.AddDays(2), 0, 0, 4, 0.5m, 0),
                    Quote.Create("TST.L", DateTime.Today.AddDays(3), 0, 0, 3, 0.2m, 0)
                };

        [Test]
        public void PositiveDirectionalMovementTest() {
            var expectedResult = new Dictionary<DateTime, decimal>
                {
                    {DateTime.Today.AddDays(1), 2},
                    {DateTime.Today.AddDays(2), 1},
                    {DateTime.Today.AddDays(3), 0},
                };

            this.Test(DirectionalMovementCalculator.Type.Plus, expectedResult);
        }

        [Test]
        public void NegativeDirectionalMovementTest()
        {
            var expectedResult = new Dictionary<DateTime, decimal>
                {
                    {DateTime.Today.AddDays(1), 0},
                    {DateTime.Today.AddDays(2), 0.5m},
                    {DateTime.Today.AddDays(3), 0.3m},
                };

            this.Test(DirectionalMovementCalculator.Type.Minus, expectedResult);
        }

        [Test]
        public void CalculateFromDateTest() {
            var expectedResult = new Dictionary<DateTime, decimal>
                {
                    {DateTime.Today.AddDays(3), 0.3m},
                };

            var datapoints = this.quotes.Select(DataPoints.CreateFromQuote);
            var calculator = new DirectionalMovementCalculator(datapoints, DirectionalMovementCalculator.Type.Minus);
            var result = calculator.CalculateAsync(DateTime.Today.AddDays(3)).Result;

            Assert.AreEqual(expectedResult, result);
        }

        private void Test(DirectionalMovementCalculator.Type type, Dictionary<DateTime, decimal> expectedResult) {
            var datapoints = this.quotes.Select(DataPoints.CreateFromQuote);
            var calculator = new DirectionalMovementCalculator(datapoints, type);
            var result = calculator.CalculateAsync().Result;

            Assert.AreEqual(expectedResult, result);
        }
    }
}
