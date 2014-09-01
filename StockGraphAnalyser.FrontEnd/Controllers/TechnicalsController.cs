
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System.Web.Mvc;
    using Domain.Service.Interfaces;
    using Models.Views;

    public class TechnicalsController : Controller
    {
        private readonly ICandleStickSignalManagementService candleStickSignalManagementService;

        public TechnicalsController(ICandleStickSignalManagementService candleStickSignalManagementService) {
            this.candleStickSignalManagementService = candleStickSignalManagementService;
        }

        public ActionResult Index(string symbol) {
            var signals = this.candleStickSignalManagementService.GetAllSignalsForCompany(symbol);
            return View("Index", new TechnicalsViewModel(symbol, signals));
        }

    }
}
