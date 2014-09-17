
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain;
    using Domain.Repository.Interfaces;
    using Models.Views;
    using Processing;

    public class TradeSignalsController : Controller
    {
        private readonly ITradeSignalRepository tradeSignalRepository;

        public TradeSignalsController(ITradeSignalRepository tradeSignalRepository) {
            this.tradeSignalRepository = tradeSignalRepository;
        }

        public ActionResult TradePerformanceHistory(string symbol) {
            var trades = this.tradeSignalRepository.GetAllForCompany(symbol);
            var model = new List<TradeHistoryPerformanceEntry>();
            Signal lastTrade = null;

            foreach (var trade in trades)
            {                
                model.Add(new TradeHistoryPerformanceEntry(
                    trade.Date, 
                    trade.CurrentEquity, 
                    trade.SignalType, 
                    lastTrade != null ? MathExtras.PercentageDifferenceBetween(lastTrade.CurrentEquity.Value, trade.CurrentEquity.Value) : 0,
                    trade.Price));
                lastTrade = trade;
            }

            return this.View("TradePerformanceHistory", model);
        }

    }
}
