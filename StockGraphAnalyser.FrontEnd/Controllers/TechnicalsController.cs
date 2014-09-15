
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Linq;
    using Domain.Service.Interfaces;
    using Models.Views;
    using Signals;

    public class TechnicalsController : Controller
    {
        private readonly ICompanyDataManagementService companyDataManagementService;
        private readonly IDataPointManagementService dataPointManagementService;
        private readonly ITradeSignalManagementService tradeSignalManagementService;

        public TechnicalsController(ICompanyDataManagementService companyDataManagementService, IDataPointManagementService dataPointManagementService, ITradeSignalManagementService tradeSignalManagementService) {
            this.companyDataManagementService = companyDataManagementService;
            this.dataPointManagementService = dataPointManagementService;
            this.tradeSignalManagementService = tradeSignalManagementService;
        }

        public ActionResult Index(string symbol) {
            var company = this.companyDataManagementService.GetBySymbol(symbol);
            var tradeSignals = this.tradeSignalManagementService.GetAll(symbol);
            var totaller = new SignalEquityPositionTotaller(tradeSignals, 100);
            var totals = totaller.Calculate();
            var percentage = totals.Any() ? totals.Last().Value - 100 : 0;
            return View("Index", new TechnicalsViewModel(symbol, company, percentage));
        }

        [HttpPost]
        public ActionResult Exclude(string id) {
            var company = this.companyDataManagementService.GetById(id);
            company.ExcludeYn = company.ExcludeYn == 1 ? (short)0 : (short)1;
            this.companyDataManagementService.Update(company);
            return this.RedirectToAction("Index", new {symbol = company.Symbol});
        }

        [HttpPost]
        public ActionResult RefreshChart(string symbol) {
            this.dataPointManagementService.InsertNewQuotesToDb(symbol);
            return this.RedirectToAction("Index", new { symbol = symbol });
        }
    }
}
