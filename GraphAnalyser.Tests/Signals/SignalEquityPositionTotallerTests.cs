
namespace GraphAnalyser.Tests.Signals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository;
    using StockGraphAnalyser.Domain.Service;
    using StockGraphAnalyser.Signals;

    [TestFixture]
    public class SignalEquityPositionTotallerTests
    {
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

        [Test]
        public void ForALaugh()
        {
            var repository = new TradeSignalRepository();
            var signals = repository.GetAllForCompany("ULVR.L").OrderBy(d => d.Date);
            var results = new SignalEquityPositionTotaller(signals, 1000).Calculate();
            foreach (var signal in signals)
            {
                var equityTotal = results.First(s => s.Key == signal.Date);
                Console.WriteLine(string.Format("Said {0} on {1} at a price of {2}. Equity currently = {3}", signal.SignalType, signal.Date.ToString("dd/MM/yyyy"), signal.Price, Math.Round(equityTotal.Value, 2)));
            }
        }
    }
}
