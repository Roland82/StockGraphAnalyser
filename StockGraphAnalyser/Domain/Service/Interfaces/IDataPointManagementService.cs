namespace StockGraphAnalyser.Domain.Service.Interfaces
{
    using System.Collections.Generic;

    public interface IDataPointManagementService
    {
        /// <summary>Inserts the new quotes into database.</summary>
        /// <param name="symbol">The symbol.</param>
        void InsertNewQuotesToDb(string symbol);

        void UpdateDatapoints(Company.ConstituentOfIndex index);

        IEnumerable<DataPoints> FindAll(Company.ConstituentOfIndex[] indexes);
    }
}