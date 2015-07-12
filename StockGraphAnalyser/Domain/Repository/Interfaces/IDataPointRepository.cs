namespace StockGraphAnalyser.Domain.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Cassandra;

    public interface IDataPointRepository
    {
        Task<RowSet> InsertAll(IEnumerable<DataPoints> dataPoints);
        Task<IEnumerable<DataPoints>> FindAll(string symbol);
        IEnumerable<DataPoints> FindAll(Company.ConstituentOfIndex[] indexes, bool omitExcluded);
        void UpdateAll(IEnumerable<DataPoints> dataPoints);
        DateTime? FindLatestDataPointDateForSymbol(string symbol);
    }
}