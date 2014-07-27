
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System.Web.Mvc;
    using StockGraphAnalyser.FrontEnd.Models.Views;

    public class TechnicalsController : Controller
    {
        public ActionResult Index(string symbol)
        {
            return View("Index", new TechnicalsViewModel { Ticker = symbol });
        }

    }
}
