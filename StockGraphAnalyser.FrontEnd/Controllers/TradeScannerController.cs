
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using Domain;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain.Service;
    using Domain.Service.Interfaces;

    public class TradeScannerController : Controller
    {
        private readonly ITradeSignalManagementService tradingSignalService;

        public TradeScannerController(ITradeSignalManagementService tradingSignalService)
        {
            this.tradingSignalService = tradingSignalService;
        }

        public ActionResult Index() {
            var signals = this.tradingSignalService.GetLatestSignals(DateTime.Today.AddDays(-14));
            return View("Index", signals);
        }
    }
}
