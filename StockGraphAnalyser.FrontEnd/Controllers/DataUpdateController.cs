namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using System.Web.Mvc;
    using Domain.Repository;
    using Domain.Service;
    using Domain.Web;
    using Processing.Calculators;
    using Processing.Types;

    public class DataUpdateController : Controller
    {
        private readonly DataPointManagementService dataManagementService = new DataPointManagementService(new DataPointRepository(), new YahooStockQuoteServiceClient(), new CalculatorFactory());
        private readonly CompanyDataManagementService companyDataManagementService = new CompanyDataManagementService(new CompanyFinderService(), new CompanyRepository());
        
        [HttpGet]
        public ActionResult Index() {
            return this.View("Index");
        }

        [HttpPost]
        public ActionResult Update(string symbol)
        {
            dataManagementService.InsertNewQuotesToDb(symbol);
            dataManagementService.FillInMissingProcessedData(symbol);
            return this.View("Update");
        }

        [HttpPost]
        public ActionResult UpdateDatapoints(int? index)
        {
            var companyRepository = new CompanyRepository();
            var companies =  companyRepository.FindByIndex(Company.ConstituentOfIndex.Ftse100);
            foreach (var company in companies)
            {
                try
                {
                    dataManagementService.InsertNewQuotesToDb(company.Symbol);
                    dataManagementService.FillInMissingProcessedData(company.Symbol);
                }
                catch (Exception)
                {
                    //TODO: Remove this once bug fixed
                }
            }

            return this.View("Update"); 
        }

        [HttpPost]
        public ActionResult UpdateCompanies()
        {
            companyDataManagementService.GetNewCompanies();
            return this.View("Update");
        }
    }
}