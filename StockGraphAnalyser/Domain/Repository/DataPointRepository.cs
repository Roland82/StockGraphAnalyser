

namespace StockGraphAnalyser.Domain.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Dapper;
    using Interfaces;

    public class DataPointRepository  : AbstractRepository, IDataPointRepository
    {
        public void InsertAll(IEnumerable<DataPoints> dataPoints)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT INTO DataPoints (Id,Symbol,Date,[Open],[Close],High,Low,Volume,MovingAverageTwoHundredDay,
                                            MovingAverageFiftyDay,MovingAverageTwentyDay,EmaTwentyTwoDay,EmaTwelveDay,TwelveDayVsTwentyDayEmaHistogram,
                                            UpperBollingerBandTwoDeviation,UpperBollingerBandOneDeviation,
                                            LowerBollingerBandTwoDeviation,LowerBollingerBandOneDeviation,
                                            ForceIndexOnePeriod, ForceIndexThirteenPeriod, IsProcessed) 
                                    VALUES (@Id,@Symbol,@Date,@Open,@Close,@High,@Low,@Volume,@MovingAverageTwoHundredDay,
                                            @MovingAverageFiftyDay,@MovingAverageTwentyDay,@EmaTwentyTwoDay,@EmaTwelveDay,@TwelveDayVsTwentyDayEmaHistogram,
                                            @UpperBollingerBandTwoDeviation,@UpperBollingerBandOneDeviation,
                                            @LowerBollingerBandTwoDeviation,@LowerBollingerBandOneDeviation,
                                            @ForceIndexOnePeriod, @ForceIndexThirteenPeriod, @IsProcessed)", dataPoints); 
            }
        }

        public IEnumerable<DataPoints> FindAll(string symbol)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
               connection.Open();
                return connection.Query<DataPoints>(string.Format("SELECT * FROM DataPoints WHERE Symbol = '{0}'", symbol));
            }
        }

        public DateTime? FindLatestDataPointDateForSymbol(string symbol) {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                return connection.Query<DateTime>(string.Format("SELECT Max(Date) FROM DataPoints WHERE Symbol = '{0}' GROUP BY Symbol", symbol)).FirstOrDefault();
            }
        } 

        public void UpdateAll(IEnumerable<DataPoints> dataPoints)
        {
            using (IDbConnection connection = new SqlConnection(this.connectionString))
            {
                
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var dataPoint in dataPoints)
                    {
                        transaction.Connection.Execute(@"UPDATE DataPoints 
                                            SET MovingAverageTwoHundredDay = @MovingAverageTwoHundredDay, 
                                            MovingAverageTwentyDay = @MovingAverageTwentyDay,
                                            MovingAverageFiftyDay = @MovingAverageFiftyDay,
                                            EmaTwentyTwoDay = @EmaTwentyTwoDay,
                                            EmaTwelveDay = @EmaTwelveDay,
                                            TwelveDayVsTwentyDayEmaHistogram = @TwelveDayVsTwentyDayEmaHistogram,
                                            UpperBollingerBandTwoDeviation = @UpperBollingerBandTwoDeviation,
                                            LowerBollingerBandTwoDeviation = @LowerBollingerBandTwoDeviation,
                                            UpperBollingerBandOneDeviation = @UpperBollingerBandOneDeviation,
                                            LowerBollingerBandOneDeviation = @LowerBollingerBandOneDeviation,
                                            ForceIndexOnePeriod = @ForceIndexOnePeriod, 
                                            ForceIndexThirteenPeriod = @ForceIndexThirteenPeriod,
                                            IsProcessed = @IsProcessed
                                            WHERE Id = @Id",
                                           new{
                                                  ForceIndexOnePeriod = dataPoint.ForceIndexOnePeriod,
                                                  ForceIndexThirteenPeriod = dataPoint.ForceIndexThirteenPeriod,
                                                  MovingAverageTwoHundredDay = dataPoint.MovingAverageTwoHundredDay,
                                                  MovingAverageFiftyDay = dataPoint.MovingAverageFiftyDay,
                                                  MovingAverageTwentyDay = dataPoint.MovingAverageTwentyDay,
                                                  EmaTwentyTwoDay = dataPoint.EmaTwentyTwoDay,
                                                  EmaTwelveDay = dataPoint.EmaTwelveDay,
                                                  TwelveDayVsTwentyDayEmaHistogram = dataPoint.TwelveDayVsTwentyDayEmaHistogram,
                                                  LowerBollingerBandTwoDeviation = dataPoint.LowerBollingerBandTwoDeviation,
                                                  UpperBollingerBandTwoDeviation = dataPoint.UpperBollingerBandTwoDeviation,
                                                  LowerBollingerBandOneDeviation = dataPoint.LowerBollingerBandOneDeviation,
                                                  UpperBollingerBandOneDeviation = dataPoint.UpperBollingerBandOneDeviation,
                                                  IsProcessed = dataPoint.IsProcessed,
                                                  Id = dataPoint.Id
                                              }, transaction: transaction);
                    }

                    transaction.Commit();
                }
            }
        }
    }
}
