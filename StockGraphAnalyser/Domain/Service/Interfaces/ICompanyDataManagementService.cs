namespace StockGraphAnalyser.Domain.Service.Interfaces
{
    using System.Collections.Generic;

    public interface ICompanyDataManagementService
    {
        /// <summary>
        /// Gets the new companies that are not in the database currently.
        /// </summary>
        void GetNewCompanies();

        void Update(Company company);

        Company GetBySymbol(string symbol);

        Company GetById(string id);

        void UpdateCompanyMetaData();

        IEnumerable<Company> FindAllMatching(string matcher);
    }
}