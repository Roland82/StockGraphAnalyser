namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;

    public class ExponentialMovingAverageTests
    {
        [Test]
        public void EmaTest() {
            var closingPrices = new Dictionary<DateTime, decimal>
                {
                    {new DateTime(2010, 3, 24), 22.27m},
                    {new DateTime(2010, 3, 25), 22.19m},
                    {new DateTime(2010, 3, 26), 22.08m},
                    {new DateTime(2010, 3, 29), 22.17m},
                    {new DateTime(2010, 3, 30), 22.18m},
                    {new DateTime(2010, 3, 31), 22.13m},
                    {new DateTime(2010, 4, 1), 22.23m},
                    {new DateTime(2010, 4, 5), 22.43m},
                    {new DateTime(2010, 4, 6), 22.24m},
                    {new DateTime(2010, 4, 7), 22.29m},
                    {new DateTime(2010, 4, 8), 22.15m},
                    {new DateTime(2010, 4, 9), 22.39m},
                    {new DateTime(2010, 4, 10), 22.38m},
                    {new DateTime(2010, 4, 11), 22.61m},
                    {new DateTime(2010, 4, 12), 23.36m},
                    {new DateTime(2010, 4, 13), 24.05m}
                };

            var expectedResult = new Dictionary<DateTime, decimal>
                {
                    {new DateTime(2010, 4, 7), 22.22m},
                    {new DateTime(2010, 4, 8), 22.21m},
                    {new DateTime(2010, 4, 9), 22.24m},
                    {new DateTime(2010, 4, 10), 22.27m},
                    {new DateTime(2010, 4, 11), 22.33m},
                    {new DateTime(2010, 4, 12), 22.52m},
                    {new DateTime(2010, 4, 13), 22.80m}
                };


            var emaCalculator = new ExponentialMovingAverageCalculator(closingPrices, 10);
            var result = emaCalculator.Calculate().Result;
            Assert.AreEqual(expectedResult, result);
        }
    }
}