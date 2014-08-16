namespace StockGraphAnalyser.Domain.Web.Interfaces
{
    using System.Collections.Generic;

    public interface ICompanyFinderService
    {
        /// <summary>
        /// Gets all symbols.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetAllSymbols();

        /// <summary>
        /// Gets all symbols.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetFtseIndex(Company.ConstituentOfIndex indexType);
    }
}