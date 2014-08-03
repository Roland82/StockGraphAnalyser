namespace GraphAnalyser.Tests.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using TestUtilities;
    using NUnit.Framework;
    using StockGraphAnalyser.Processing.Calculators;

    [TestFixture]
    public class StandardDeviationCalculatorTests
    {
        [Test]
        public void Test()
        {
            var closingPrices = GraphPlottingUtilities.CreateGraph(new DateTime(2009, 5, 29), new[]{
                90.70m, 92.90m, 92.98m, 91.80m, 92.66m, 92.68m, 92.30m, 92.77m, 92.54m, 92.95m, 93.20m, 91.07m, 
                89.83m, 89.74m, 90.40m, 90.74m, 88.02m, 88.09m, 88.84m, 90.78m, 90.54m, 91.39m, 90.65m });

            var expectedResult = new Dictionary<DateTime, decimal>
                {
                    {new DateTime(2009, 6, 25), 1.64m},
                    {new DateTime(2009, 6, 26), 1.64m},
                    {new DateTime(2009, 6, 29), 1.60m},
                    {new DateTime(2009, 6, 30), 1.55m}
                };

            var calculator = new StandardDeviationCalculator(closingPrices, 20);
            var result = calculator.Calculate().Result;
            Assert.AreEqual(expectedResult, result);

        }
    }
}