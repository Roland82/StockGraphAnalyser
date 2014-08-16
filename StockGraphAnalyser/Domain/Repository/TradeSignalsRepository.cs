


namespace StockGraphAnalyser.Domain.Repository
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Dapper;
    using System;
    using System.Collections.Generic;
    using Interfaces;

    public class TradeSignalsRepository : AbstractRepository, ITradeSignalsRepository
    {
        public IEnumerable<DataPoints> FindSharesAtValue()
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                return connection.Query<DataPoints>(string.Format(@"SELECT * FROM DataPoints
                                                                    WHERE [Close] BETWEEN MovingAverageFiftyDay AND MovingAverageTwoHundredDay 
                                                                    AND Date = '{0}'", this.GetLatestDateInDataPoints().ToString("yyyy-MM-dd")));
            }
        }

        public IEnumerable<DataPoints> FindSharesBelowLowerBollingerBand()
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                return connection.Query<DataPoints>(string.Format(@" SELECT d.* FROM DataPoints d INNER JOIN Companies c on d.Symbol = c.Symbol WHERE c.[Index] >= 1 AND [Close] < LowerBollingerBandTwoDeviation AND Date = '{0}'",
                                                              this.GetLatestDateInDataPoints().ToString("yyyy-MM-dd")));
            }
        }

        public IEnumerable<DataPoints> FindSharesAboveUpperBollingerBand()
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                return connection.Query<DataPoints>(string.Format(@" SELECT d.* FROM DataPoints d INNER JOIN Companies c on d.Symbol = c.Symbol WHERE c.[Index] >= 1 AND [Close] > UpperBollingerBandTwoDeviation AND Date = '{0}'",
                                                                                     this.GetLatestDateInDataPoints().ToString("yyyy-MM-dd")));
            }
        }

        private DateTime GetLatestDateInDataPoints()
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                return DateTime.Parse(connection.Query<String>(@"SELECT Max(Date) FROM DataPoints;").First());
            }
        }
    }
}
