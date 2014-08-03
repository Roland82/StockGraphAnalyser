namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System.Web.Mvc;
    using Domain.Repository;
    using Domain.Service;
    using Domain.Web;
    using Processing.Calculators;

    public class DataUpdateController : Controller
    {
        public ActionResult Update(string symbol) {
            var dataManagementService = new DataPointManagementService(new DataPointRepository(),
                                                                       new YahooStockQuoteServiceClient(),
                                                                       new CalculatorFactory());
            dataManagementService.InsertNewQuotesToDb(symbol);
            dataManagementService.FillInMissingProcessedData(symbol);
            return this.View("Update");
        }
    }
}