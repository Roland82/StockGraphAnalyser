

namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;

    public class ForceIndexCalculatorTests
    {
        [Test]
        public void CalculateForceIndexTest() {
            var dataList = new List<Tuple<DateTime, decimal, long>>
                {
                    new Tuple<DateTime, decimal, long>(new DateTime(2010, 6, 25), 14.33m, 40000),
                    new Tuple<DateTime, decimal, long>(new DateTime(2010, 6, 28), 14.23m, 45579),
                    new Tuple<DateTime, decimal, long>(new DateTime(2010, 6, 29), 13.98m, 66285),
                    new Tuple<DateTime, decimal, long>(new DateTime(2010, 6, 30), 13.96m, 51761),
                    new Tuple<DateTime, decimal, long>(new DateTime(2010, 7, 1), 13.93m, 69341),
                    new Tuple<DateTime, decimal, long>(new DateTime(2010, 7, 2), 13.84m, 41631),
                    new Tuple<DateTime, decimal, long>(new DateTime(2010, 7, 6), 13.99m, 73499)
                };

            var expectedOutput = new Dictionary<DateTime, decimal?>
                {
                    {new DateTime(2010, 6, 28), -4557.90m},
                    {new DateTime(2010, 6, 29), -16571.25m},
                    {new DateTime(2010, 6, 30), -1035.22m},
                    {new DateTime(2010, 7, 1), -2080.23m},
                    {new DateTime(2010, 7, 2), -3746.79m},
                    {new DateTime(2010, 7, 6), 11024.85m}
                };

            var calculator = new ForceIndexCalculator(dataList);
            var result = calculator.Calculate().Result;
            Assert.AreEqual(expectedOutput, result);
        }
    }
}