namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using System.Web.Mvc;
    using Domain;
    using Domain.Service.Interfaces;

    public class DataUpdateController : Controller
    {
        private readonly IDataPointManagementService dataManagementService;
        private readonly ICompanyDataManagementService companyDataManagementService;
        private readonly ICandleStickSignalManagementService candleStickSignalManagementService;
        private readonly ITradeSignalManagementService tradeSignalManagementService;

        public DataUpdateController(IDataPointManagementService dataManagementService, ICompanyDataManagementService companyDataManagementService, ICandleStickSignalManagementService candleStickSignalManagementService, ITradeSignalManagementService tradeSignalManagementService)
        {
            this.dataManagementService = dataManagementService;
            this.companyDataManagementService = companyDataManagementService;
            this.candleStickSignalManagementService = candleStickSignalManagementService;
            this.tradeSignalManagementService = tradeSignalManagementService;
        }

        [HttpGet]
        public ActionResult Index() {
            return this.View("Index");
        }


        [HttpPost]
        public ActionResult Update(string symbol)
        {
            dataManagementService.InsertNewQuotesToDb(symbol);
            this.TempData["message"] = string.Format("Datapoints Updated for {0}", symbol);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateCandlestickSignals() {
            this.candleStickSignalManagementService.GenerateLatestSignals(DateTime.MinValue);
            this.TempData["message"] = "Candlesticks Updated";
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateTradeSignals(string symbol)
        {
            this.tradeSignalManagementService.GenerateNewSignals(symbol);
            this.TempData["message"] = "Trading Signals Update for " + symbol;
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateTradeSignalsForAll()
        {
            this.tradeSignalManagementService.GenerateNewSignals();
            this.TempData["message"] = "Trading signals Updated";
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateDatapoints(Company.ConstituentOfIndex? index)
        {
            this.dataManagementService.UpdateDatapoints(index.Value);
            this.TempData["message"] = "Datapoints Updated For Index " + index;
            return this.RedirectToAction("Index"); 
        }

        [HttpPost]
        public ActionResult UpdateCompanyMetaData()
        {
            this.companyDataManagementService.UpdateCompanyMetaData();
            this.TempData["message"] = "Company Metadata updated";
            return this.RedirectToAction("Index"); 
        }

        [HttpPost]
        public ActionResult UpdateCompanies()
        {
            companyDataManagementService.GetNewCompanies();
            this.TempData["message"] = "Updated new companies";
            return this.RedirectToAction("Index");
        }
    }
}