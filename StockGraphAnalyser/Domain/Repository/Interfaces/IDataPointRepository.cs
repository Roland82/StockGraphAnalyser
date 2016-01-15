namespace StockGraphAnalyser.Domain.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IDataPointRepository
    {
        void InsertAll(IEnumerable<DataPoints> dataPoints);
        IEnumerable<DataPoints> FindAll(string symbol);
        IEnumerable<DataPoints> FindAll(Company.ConstituentOfIndex[] indexes);
        void UpdateAll(IEnumerable<DataPoints> dataPoints);
        DateTime? FindLatestDataPointDateForSymbol(string symbol);
    }
}