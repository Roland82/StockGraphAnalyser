namespace StockGraphAnalyser.Domain.Service.Interfaces
{
    public interface ICompanyDataManagementService
    {
        /// <summary>
        /// Gets the new companies that are not in the database currently.
        /// </summary>
        void GetNewCompanies();

        void UpdateCompanyMetaData();
    }
}