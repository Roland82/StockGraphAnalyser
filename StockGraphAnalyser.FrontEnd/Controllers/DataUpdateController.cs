namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain;
    using Domain.Repository;
    using Domain.Service;
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
            return this.View("Update");
        }

        [HttpPost]
        public ActionResult UpdateCandlestickSignals() {
            this.candleStickSignalManagementService.GenerateLatestSignals(DateTime.MinValue);
            return this.View("Update");
        }

        [HttpPost]
        public ActionResult UpdateTradeSignals()
        {
            this.tradeSignalManagementService.GenerateNewSignals();
            return this.View("Update");
        }

        [HttpPost]
        public ActionResult UpdateDatapoints(int? index)
        {
            // TODO This can be done in one select
            var companyRepository = new CompanyRepository();
            var companyList = new List<Company>();
            companyList.AddRange(companyRepository.FindByIndex(Company.ConstituentOfIndex.Ftse100));
            companyList.AddRange(companyRepository.FindByIndex(Company.ConstituentOfIndex.Ftse250));
            companyList.AddRange(companyRepository.FindByIndex(Company.ConstituentOfIndex.SmallCap));
            foreach (var company in companyList)
            {
                try
                {
                    dataManagementService.InsertNewQuotesToDb(company.Symbol);
                }
                catch (Exception)
                {
                    //TODO: Remove this once bug fixed
                }
            }

            return this.View("Update"); 
        }

        [HttpPost]
        public ActionResult UpdateCompanyMetaData()
        {
            this.companyDataManagementService.UpdateCompanyMetaData();
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