
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using Domain;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain.Service;
    using Domain.Service.Interfaces;

    public class TradeScannerController : Controller
    {
        private readonly ITradingSignalService tradingSignalService;

        public TradeScannerController(ITradingSignalService tradingSignalService) {
            this.tradingSignalService = tradingSignalService;
        }

        public ActionResult Index()
        {
            var tradeSignalsDictionary = new Dictionary<string, IEnumerable<DataPoints>>();
            tradeSignalsDictionary.Add("In Value Zone", this.tradingSignalService.GetDatapointsForTradeSignal(TradingSignalService.SignalType.InValueZone));
            tradeSignalsDictionary.Add("Below Lower Bollinger Band", this.tradingSignalService.GetDatapointsForTradeSignal(TradingSignalService.SignalType.BelowLowerBollingerBand));
            tradeSignalsDictionary.Add("Above Upper Bollinger Band", this.tradingSignalService.GetDatapointsForTradeSignal(TradingSignalService.SignalType.AboveUpperBollingerBand));
            return View("Index", tradeSignalsDictionary);
        }
    }
}
