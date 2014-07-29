
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System.Web.Mvc;
    using StockGraphAnalyser.Domain.Repository;
    using StockGraphAnalyser.Domain.Service;
    using StockGraphAnalyser.Domain.Web;

    public class DataUpdateController : Controller
    {
        public ActionResult Update(string symbol)
        {
            var dataManagementService = new DataPointManagementService(new DataPointRepository(), new YahooStockQuoteServiceClient());
            dataManagementService.InsertNewQuotesToDb(symbol);
            dataManagementService.FillInMissingProcessedData(symbol);
            return this.View("Update");
        }
    }
}
