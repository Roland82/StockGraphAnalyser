
namespace GraphAnalyser.Tests.Signals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository;
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

        [Test]
        public void ForALaugh()
        {
            var repository = new TradeSignalRepository();
            var companies = new CompanyRepository().FindByIndex(Company.ConstituentOfIndex.SmallCap);
            var totalEquityUsed = 0m;
            var totalEndEquity = 0m;
            foreach (var company in companies)
            {
                var signals = repository.GetAllForCompany(company.Symbol).OrderBy(d => d.Date);
                if (signals.Any())
                {
                    var results = new SignalEquityPositionTotaller(repository.GetAllForCompany(company.Symbol).OrderBy(d => d.Date), 100).Calculate();
                    if (results.OrderBy(d => d.Key).Last().Value < 700 && results.OrderBy(d => d.Key).Last().Value > -700)
                    {
                        totalEquityUsed = totalEquityUsed + 100;
                        totalEndEquity = totalEndEquity + results.OrderBy(d => d.Key).Last().Value;
                        Console.WriteLine("For company {0}. Start Equity {1}. End Equity {2}", company.Symbol, 100, Math.Round(results.OrderBy(d => d.Key).Last().Value, 2));
                    }
                }
            }

            Console.WriteLine("Total Equity Used = {0}. End Result = {1}", totalEquityUsed, totalEndEquity);
        }

        [Test]
        public void ForALaugh2()
        {
            var repository = new TradeSignalRepository();
            var trades = repository.GetAllForCompany("BDEV.L").OrderBy(d => d.Date);
            var totals = new SignalEquityPositionTotaller(trades, 100).Calculate();
            foreach (var total in totals)
            {
                var trade = trades.First(t => t.Date == total.Key);
                Console.WriteLine("{0}: {1} at {2}. Current Equity {3}", total.Key, trade.SignalType, trade.Price, Math.Round(total.Value, 2));
            }
        }
    }
}
