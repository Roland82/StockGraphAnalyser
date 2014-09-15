
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Domain.Repository;
    using Domain.Repository.Interfaces;
    using Models.Views;
    using Processing;
    using Signals;

    public class TradeSignalsController : Controller
    {
        private readonly ITradeSignalRepository tradeSignalRepository;

        public TradeSignalsController(ITradeSignalRepository tradeSignalRepository) {
            this.tradeSignalRepository = tradeSignalRepository;
        }

        public ActionResult TradePerformanceHistory(string symbol) {
            var previousEquity = 100m;
            var trades = this.tradeSignalRepository.GetAllForCompany(symbol);
            var totals = new SignalEquityPositionTotaller(trades, previousEquity).Calculate();
            var model = new List<TradeHistoryPerformanceEntry>();

            foreach (var total in totals)
            {                
                var trade = trades.First(t => t.Date == total.Key);
                model.Add(new TradeHistoryPerformanceEntry(
                    trade.Date, 
                    total.Value, 
                    trade.SignalType, 
                    MathExtras.PercentageDifferenceBetween(total.Value, previousEquity),
                    trade.Price));
                previousEquity = total.Value;
            }

            return this.View("TradePerformanceHistory", model);
        }

    }
}
