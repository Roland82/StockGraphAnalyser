

namespace StockGraphAnalyser.Domain.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface ITradeSignalRepository
    {
        void InsertAll(IEnumerable<Signal> signals);
        void DeleteAll();
        void DeleteAll(string symbol);
        IEnumerable<Signal> GetAllForCompany(string symbol);
        IEnumerable<Signal> GetAll(DateTime fromDate);
    }
}
