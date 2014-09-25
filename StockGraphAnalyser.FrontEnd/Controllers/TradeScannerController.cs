
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using System.Web.Mvc;
    using Domain.Service.Interfaces;

    public class TradeScannerController : Controller
    {
        private readonly ITradeSignalManagementService tradingSignalService;

        public TradeScannerController(ITradeSignalManagementService tradingSignalService)
        {
            this.tradingSignalService = tradingSignalService;
        }

        public ActionResult Index() {
            var signals = this.tradingSignalService.GetAll(DateTime.Today.AddDays(-365));
            return View("Index", signals);
        }
    }
}
