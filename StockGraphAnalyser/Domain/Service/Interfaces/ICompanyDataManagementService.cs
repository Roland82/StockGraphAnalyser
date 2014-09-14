namespace StockGraphAnalyser.Domain.Service.Interfaces
{
    using System.Collections.Generic;

    public interface ICompanyDataManagementService
    {
        /// <summary>
        /// Gets the new companies that are not in the database currently.
        /// </summary>
        void GetNewCompanies();

        void UpdateCompanyMetaData();

        IEnumerable<Company> FindAllMatching(string matcher);
    }
}