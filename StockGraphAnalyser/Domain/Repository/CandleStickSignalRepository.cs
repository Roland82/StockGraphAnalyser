

namespace StockGraphAnalyser.Domain.Repository
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Dapper;
    using Interfaces;

    public class CandleStickSignalRepository : AbstractRepository, ICandleStickSignalRepository
    {
        public void InsertAll(IEnumerable<CandleStickSignal> signals)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT INTO CandlestickSignals (@Id, @Symbol, @Date, @CandleStickSignalType)", signals);
            }
        }

        public IEnumerable<CandleStickSignal> FindAllForCompany(string symbol)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<CandleStickSignal>(string.Format("SELECT * FROM CandlestickSignals WHERE Symbol  ='{0}'", symbol));
            }
        }
    }
}
