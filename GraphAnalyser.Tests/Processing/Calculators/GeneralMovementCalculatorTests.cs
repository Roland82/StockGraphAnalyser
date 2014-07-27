

namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;

    [TestFixture]
    public class GeneralMovementCalculatorTests
    {
        private readonly Dictionary<DateTime, decimal> priceAction = new Dictionary<DateTime, decimal>
            {
                { DateTime.Today.AddDays(1), 1m },
                { DateTime.Today.AddDays(2), 2m },
                { DateTime.Today.AddDays(3), 3m },
                { DateTime.Today.AddDays(4), 4m },
                { DateTime.Today.AddDays(5), 2m },
                { DateTime.Today.AddDays(6), 1m }
            };

        [Test]
        public void SixDayThresholdTest() {
            var calculator = new GeneralMovementCalculator(this.priceAction, 6);
            var expectedResult = new Dictionary<DateTime, decimal> {{DateTime.Today.AddDays(6), 0}};
            Assert.AreEqual(expectedResult, calculator.Calculate().Result);
        }

        [Test]
        public void TwoDayThresholdTest()
        {
            /*
             *  
                { DateTime.Today.AddDays(1), 1m },
                { DateTime.Today.AddDays(2), 2m },
                { DateTime.Today.AddDays(3), 3m },
                { DateTime.Today.AddDays(4), 4m },
                { DateTime.Today.AddDays(5), 2m },
                { DateTime.Today.AddDays(6), 1m }
             */
            var calculator = new GeneralMovementCalculator(this.priceAction, 2);
            var expectedResult = new Dictionary<DateTime, decimal>
                {
                    {DateTime.Today.AddDays(2), 0},
                    {DateTime.Today.AddDays(3), 0},
                    {DateTime.Today.AddDays(4), 0},
                    {DateTime.Today.AddDays(5), 0},
                    {DateTime.Today.AddDays(6), 0}
                };

            Assert.AreEqual(expectedResult, calculator.Calculate().Result);
        }
    }
}
