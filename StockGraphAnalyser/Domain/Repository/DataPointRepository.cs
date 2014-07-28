

namespace StockGraphAnalyser.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Dapper;
    using Interfaces;

    public class DataPointRepository : IDataPointRepository
    {
        private const string ConnectionString = @"Server=ROLAND-PC\SQLEXPRESS;Database=StockGraphAnalyser;Trusted_Connection=True;";

        public void InsertAll(IEnumerable<DataPoints> dataPoints)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT INTO DataPoints VALUES (@Id,@Symbol,@Date,@Open,@Close,@High,@Low,@MovingAverageTwoHundredDay,@MovingAverageFiftyDay,@UpperBollingerBand,@LowerBollingerBand)", dataPoints); 
            }
        }

        public IEnumerable<DataPoints> FindAll(string symbol)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
               connection.Open();
                return connection.Query<DataPoints>(string.Format("SELECT * FROM DataPoints WHERE Symbol = '{0}'", symbol));
            }
        }

        public DateTime? FindLatestDataPointDateForSymbol(string symbol) {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    return transaction.Connection.Query<DateTime>("SELECT Max(Date) FROM DataPoints WHERE Symbol = '{0}' GROUP BY Symbol", symbol).FirstOrDefault();
                }
            }
        } 

        public void UpdateAll(IEnumerable<DataPoints> dataPoints)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var dataPoint in dataPoints)
                    {
                        transaction.Connection.Execute(@"UPDATE DataPoints 
                                            SET MovingAverageTwoHundredDay = @MovingAverageTwoHundredDay, 
                                            MovingAverageFiftyDay = @MovingAverageFiftyDay,
                                            UpperBollingerBand = @UpperBollingerBand,
                                            LowerBollingerBand = @LowerBollingerBand
                                            WHERE Id = @Id",
                                           new{
                                                  MovingAverageTwoHundredDay = dataPoint.MovingAverageTwoHundredDay,
                                                  MovingAverageFiftyDay = dataPoint.MovingAverageFiftyDay,
                                                  LowerBollingerBand = dataPoint.LowerBollingerBand,
                                                  UpperBollingerBand = dataPoint.UpperBollingerBand,
                                                  Id = dataPoint.Id
                                              }, transaction: transaction);
                    }

                    transaction.Commit();
                }
            }
        }
    }
}
