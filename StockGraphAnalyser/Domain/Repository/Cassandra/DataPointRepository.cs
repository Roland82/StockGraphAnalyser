

namespace StockGraphAnalyser.Domain.Repository.Cassandra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using StockGraphAnalyser.Domain.Repository.Interfaces;
    using global::Cassandra;

    public class DataPointRepository : IDataPointRepository
    {
        private readonly IConnectionManager connection = ConnectionManager.Instance;

        public Task<RowSet> InsertAll(IEnumerable<DataPoints> dataPoints) {
            const string statement = @"UPDATE datapoints SET ""Open"" = ?,""Close"" = ?,""High"" = ?,""Low"" = ?,""Volume"" = ?,
                                            ""MovingAverageTwoHundredDay"" = ?,""MovingAverageFiftyDay"" = ?,""MovingAverageTwentyDay"" = ?,
                                            ""MacdTwentyTwoOverTwelveDay"" = ?,""MacdTwentyTwoOverTwelveDaySignalLine"" = ?,""MacdTwentyTwoOverTwelveDayHistogram"" = ?, 
                                            ""UpperBollingerBandTwoDeviation"" = ?,""UpperBollingerBandOneDeviation"" = ?,
                                            ""LowerBollingerBandTwoDeviation"" = ?,""LowerBollingerBandOneDeviation"" = ?,
                                            ""ForceIndexOnePeriod"" = ?,""ForceIndexThirteenPeriod"" = ?,""IsProcessed"" = ? WHERE ""Symbol"" = ? AND ""Date"" = ?";

            var ps = connection.Session.Prepare(statement);

            var batchStatement = new BatchStatement();
            foreach (var d in dataPoints)
            {
                batchStatement.Add(ps.Bind(
                    d.Open,
                    d.Close,
                    d.High,
                    d.Low,
                    d.Volume,
                    d.MovingAverageTwoHundredDay,
                    d.MovingAverageFiftyDay,
                    d.MovingAverageTwentyDay,
                    d.MacdTwentyTwoOverTwelveDay, d.MacdTwentyTwoOverTwelveDaySignalLine,
                    d.MacdTwentyTwoOverTwelveDayHistogram,
                    d.UpperBollingerBandTwoDeviation, d.UpperBollingerBandOneDeviation,
                    d.LowerBollingerBandTwoDeviation, d.LowerBollingerBandOneDeviation,
                    d.ForceIndexOnePeriod, d.ForceIndexThirteenPeriod, d.IsProcessed,
                    d.Symbol,
                    d.Date));
            }

            return connection.Session.ExecuteAsync(batchStatement);
        }

        public Task<IEnumerable<DataPoints>> FindAll(string symbol) {
            var statement = connection.Session.Prepare(@"SELECT * FROM datapoints WHERE ""Symbol"" = ? AND ""Date"" > ?").Bind(symbol, DateTime.MinValue);
            var task = connection.Session.ExecuteAsync(statement);
            return task.ContinueWith((r) => r.Result.ToList().Select(DataPoints.CreateFromRow));
        }

        public IEnumerable<DataPoints> FindAll(Company.ConstituentOfIndex[] indexes, bool omitExcluded) {
            throw new NotImplementedException();
        }

        public void UpdateAll(IEnumerable<DataPoints> dataPoints) {
            this.InsertAll(dataPoints);
        }

        public DateTime? FindLatestDataPointDateForSymbol(string symbol) {
            throw new NotImplementedException();
        }
    }
}
