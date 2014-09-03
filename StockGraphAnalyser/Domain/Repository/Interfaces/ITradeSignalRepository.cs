

namespace StockGraphAnalyser.Domain.Repository.Interfaces
{
    using System.Collections.Generic;

    public interface ITradeSignalRepository
    {
        void InsertAll(IEnumerable<Signal> signals);
        IEnumerable<Signal> GetAllForCompany(string symbol);
    }
}
