using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository;

    public class TradeScannerController : Controller
    {
        private TradeSignalsRepository tradeSignalsRepository = new TradeSignalsRepository();

        public ActionResult Index()
        {
            var tradeSignalsDictionary = new Dictionary<string, IEnumerable<DataPoints>>();
            tradeSignalsDictionary.Add("In Value Zone", this.tradeSignalsRepository.FindSharesAtValue());
            tradeSignalsDictionary.Add("Below Lower Bollinger Band", this.tradeSignalsRepository.FindSharesBelowLowerBollingerBand());
            tradeSignalsDictionary.Add("Above Upper Bollinger Band", this.tradeSignalsRepository.FindSharesAboveUpperBollingerBand());
            return View("Index", tradeSignalsDictionary);
        }

    }
}
