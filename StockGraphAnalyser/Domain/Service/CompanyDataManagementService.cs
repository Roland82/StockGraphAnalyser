

namespace StockGraphAnalyser.Domain.Service
{
    using System.Linq;
    using StockGraphAnalyser.Domain.Repository;
    using StockGraphAnalyser.Domain.Web;

    public class CompanyDataManagementService
    {
        private SymbolFinderService symbolFinderService = new SymbolFinderService();
        private CompanyRepository companyRepository = new CompanyRepository();

        public void UpdateCompanies()
        {
            var companies = symbolFinderService.GetFtse100();
            var companiesInDb = companyRepository.FindAll();

            var updatedCompanies = companiesInDb
                .Where(c => companies.ContainsKey(c.Symbol.Replace(".L", "")))
                .Select(c => { c.Index = 1; return c; });

            companyRepository.UpdateAll(updatedCompanies);

        }
    }
}
