using System;


namespace StockGraphAnalyser.Domain.Repository.Cassandra
{
    using System.Collections.Generic;
    using StockGraphAnalyser.Domain.Repository.Interfaces;

    class CandlestickSignalRepository : ICandleStickSignalRepository
    {
        public void InsertAll(IEnumerable<CandleStickSignal> signals) {
       
        }

        public IEnumerable<CandleStickSignal> FindAllForCompany(string symbol) {
            return new List<CandleStickSignal>();
        }
    }
}
