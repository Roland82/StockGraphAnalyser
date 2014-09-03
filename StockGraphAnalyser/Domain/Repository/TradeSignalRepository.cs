
namespace StockGraphAnalyser.Domain.Repository
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Dapper;
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

        public IEnumerable<Signal> GetAllForCompany(string symbol)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                return connection.Query<Signal>(string.Format("SELECT * FROM Signals WHERE Symbol = '{0}'", symbol));
            }
        }
    }
}
