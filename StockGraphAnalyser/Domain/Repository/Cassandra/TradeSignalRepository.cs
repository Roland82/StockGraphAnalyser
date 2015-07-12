


namespace StockGraphAnalyser.Domain.Repository.Cassandra
{
    using StockGraphAnalyser.Domain.Repository.Interfaces;
    using System;
    using System.Collections.Generic;

    public class TradeSignalRepository : ITradeSignalRepository
    {
        public void InsertAll(IEnumerable<Signal> signals) {
          
        }

        public void DeleteAll() {
     
        }

        public void DeleteAll(string symbol) {
      
        }

        public IEnumerable<Signal> GetAllForCompany(string symbol) {
            return new List<Signal>();
        }

        public IEnumerable<Signal> GetAll(DateTime fromDate) {
            return new List<Signal>();
        }
    }
}
