
namespace StockGraphAnalyser.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Dapper;
    using System.Linq;
    using StockGraphAnalyser.Domain.Repository.Interfaces;

    public class TradeSignalRepository : AbstractRepository, ITradeSignalRepository
    {
        public void InsertAll(IEnumerable<Signal> signals)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT INTO Signals VALUES (@Id,@Symbol,@Date, @SignalType, @Price)", signals);
            }
        }

        public void DeleteAll() {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                connection.Execute(@"DELETE FROM Signals");
            }
        }

        public IEnumerable<Signal> GetAllForCompany(string symbol)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                return connection.Query<Signal>(string.Format("SELECT * FROM Signals WHERE Symbol = '{0}'", symbol)).OrderBy(d => d.Date);
            }
        }

        public IEnumerable<Signal> GetAll(DateTime fromDate)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                return connection.Query<Signal>(string.Format(
                    "SELECT * FROM Signals WHERE [Date] >= '{0}' ORDER BY [Date] DESC", 
                    fromDate.ToString("yyyy-MM-dd")));
            }
        }
    }
}
