

namespace StockGraphAnalyser.Domain.Service
{
    using System;
    using System.Linq;
    using Interfaces;
    using Repository.Interfaces;
    using StockDataProviders.Interfaces;

    public class CompanyDataManagementService : ICompanyDataManagementService
    {
        private readonly ICompanyFinderService companyFinderService;
        private readonly ICompanyRepository companyRepository;

        public CompanyDataManagementService(ICompanyFinderService companyFinderService, ICompanyRepository companyRepository) {
            this.companyFinderService = companyFinderService;
            this.companyRepository = companyRepository;
        }

        /// <summary>
        /// Gets the new companies that are not in the database currently.
        /// </summary>
        public void GetNewCompanies() {
            var currentCompanySymbols = this.companyRepository.FindAll().Select(c => c.Symbol);
            var allCompanies = this.companyFinderService.GetAllSymbols();
            
            var companiesToInsert = allCompanies
                .Where(c => !currentCompanySymbols.Contains(c.Key))
                .Select(c => Company.Create(c.Value, c.Key + ".L", Company.ConstituentOfIndex.Unknown));
            
            if (companiesToInsert.Any())
            {
                this.companyRepository.InsertAll(companiesToInsert);
            }
        }

        public void UpdateCompanyMetaData()
        {
            foreach (var i in Enum.GetValues(typeof(Company.ConstituentOfIndex)))
            {
                var indexType = (Company.ConstituentOfIndex) i;
                if (indexType == Company.ConstituentOfIndex.Unknown) continue;
                var companiesInIndex = this.companyFinderService.GetFtseIndex(indexType);
                var companies = this.companyRepository.FindAll();
                var updatedCompanies = companies.Where(c => companiesInIndex.ContainsKey(c.Symbol.Replace(".L", ""))).Select(c => { c.Index = indexType.GetHashCode(); return c; });
                this.companyRepository.UpdateAll(updatedCompanies);
            }
        }
    }
}

