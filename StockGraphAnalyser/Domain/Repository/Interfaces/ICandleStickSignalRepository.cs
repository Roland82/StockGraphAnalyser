namespace StockGraphAnalyser.Domain.Repository.Interfaces
{
    using System.Collections.Generic;

    public interface ICandleStickSignalRepository
    {
        void InsertAll(IEnumerable<CandleStickSignal> signals);
        IEnumerable<CandleStickSignal> FindAllForCompany(string symbol);
    }
}