
namespace GraphAnalyser.Tests.Signals
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Signals;

    [TestFixture]
    public class SignalEquityPositionTotallerTests
    {
        [Test]
        public void TakeProfitsTest()
        {
            var signals = new[]{
                Signal.Create("SGP.L", DateTime.Today, SignalType.Buy, 1000), 
                Signal.Create("SGP.L", DateTime.Today.AddDays(1), SignalType.TakeProfits, 1100),
                Signal.Create("SGP.L", DateTime.Today.AddDays(2), SignalType.Sell, 1000), 
                Signal.Create("SGP.L", DateTime.Today.AddDays(3), SignalType.Buy, 800), 
                Signal.Create("SGP.L", DateTime.Today.AddDays(4), SignalType.TakeProfits, 880), 
            };

            var totaller = new SignalEquityPositionTotaller(signals, 2000);
            var result = totaller.Calculate();
            var expectedResult = new Dictionary<DateTime, decimal>{
                                                                      {DateTime.Today.AddDays(0), 2000},
                                                                      {DateTime.Today.AddDays(1), 2200},
                                                                      {DateTime.Today.AddDays(2), 2200},
                                                                      {DateTime.Today.AddDays(3), 2640},
                                                                      {DateTime.Today.AddDays(4), 2904}
                                                                  };
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void BuyIsSucessfulTest()
        {
            var signals = new[]{
                Signal.Create("SGP.L", DateTime.Today, SignalType.Buy, 1000), 
                Signal.Create("SGP.L", DateTime.Today.AddDays(1), SignalType.Sell, 1100)
            };

            var totaller = new SignalEquityPositionTotaller(signals, 2000);
            var result = totaller.Calculate();
            var expectedResult = new Dictionary<DateTime, decimal>{
                                                                      {DateTime.Today.AddDays(0), 2000},
                                                                      {DateTime.Today.AddDays(1), 2200}
                                                                  };
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ShortIsSucessfulTest()
        {
            var signals = new[]{
                Signal.Create("SGP.L", DateTime.Today, SignalType.Sell, 1000), 
                Signal.Create("SGP.L", DateTime.Today.AddDays(1), SignalType.Buy, 900)
            };

            var totaller = new SignalEquityPositionTotaller(signals, 2000);
            var result = totaller.Calculate();
            var expectedResult = new Dictionary<DateTime, decimal>{
                                                                      {DateTime.Today.AddDays(0), 2000},
                                                                      {DateTime.Today.AddDays(1), 2200}
                                                                  };
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ShortIsNotSucessfulTest()
        {
            var signals = new[]{
                Signal.Create("SGP.L", DateTime.Today, SignalType.Sell, 1000), 
                Signal.Create("SGP.L", DateTime.Today.AddDays(1), SignalType.Buy, 1100)
            };

            var totaller = new SignalEquityPositionTotaller(signals, 2000);
            var result = totaller.Calculate();
            var expectedResult = new Dictionary<DateTime, decimal>{
                                                                      {DateTime.Today.AddDays(0), 2000},
                                                                      {DateTime.Today.AddDays(1), 1800}
                                                                  };
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void BuyIsNotSucessfulTest()
        {
            var signals = new[]{
                Signal.Create("SGP.L", DateTime.Today, SignalType.Buy, 1000), 
                Signal.Create("SGP.L", DateTime.Today.AddDays(1), SignalType.Sell, 900)
            };

            var totaller = new SignalEquityPositionTotaller(signals, 2000);
            var result = totaller.Calculate();
            var expectedResult = new Dictionary<DateTime, decimal>{
                                                                      {DateTime.Today.AddDays(0), 2000},
                                                                      {DateTime.Today.AddDays(1), 1800}
                                                                  };
            Assert.AreEqual(expectedResult, result);
        }
    }
}
