namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System.Web.Mvc;
    using Domain.Repository;
    using Domain.Service;
    using Domain.Web;
    using Processing.Calculators;
    using StockGraphAnalyser.Processing.Types;

    public class DataUpdateController : Controller
    {
        private DataPointManagementService dataManagementService = new DataPointManagementService(new DataPointRepository(), new YahooStockQuoteServiceClient(), new CalculatorFactory());

        public ActionResult Update(string symbol)
        {
            dataManagementService.InsertNewQuotesToDb(symbol);
            dataManagementService.FillInMissingProcessedData(symbol);
            return this.View("Update");
        }

        public ActionResult UpdateIndex(int index)
        {
            var companyRepository = new CompanyRepository();
            var companies = companyRepository.FindByIndex(Company.ConstituentOfIndex.Ftse100);
            foreach (var company in companies)
            {

                dataManagementService.InsertNewQuotesToDb(company.Symbol);
                dataManagementService.FillInMissingProcessedData(company.Symbol);
            }

            return this.View("Update"); 
        }

        public ActionResult UpdateCompanies()
        {
            var dataManagementService = new CompanyDataManagementService();
            dataManagementService.UpdateCompanies();
            return this.View("Update");
        }
    }
}