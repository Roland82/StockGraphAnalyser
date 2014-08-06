

namespace StockGraphAnalyser.Domain.Service
{
    using System.Linq;
    using Processing.Types;
    using Repository.Interfaces;
    using Web.Interfaces;

    public class CompanyDataManagementService
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
    }
}
