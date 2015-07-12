
namespace GraphAnalyser.Tests.Domain.Repository.Cassandra
{
    using System;
    using System.Collections.Generic;
    using StockGraphAnalyser.Domain.Repository.Cassandra;
    using StockGraphAnalyser.Processing;
    using global::Cassandra;

    public class AbstractCassandraRepository
    {
        public readonly IConnectionManager Connection = ConnectionManager.Instance;
        private const int BatchSize = 500;

        protected void SafeBatch<T>(IEnumerable<T> list, Action<BatchStatement, T> executer) {
            var batches = list.GroupBatches(BatchSize);
            foreach (var batch in batches)
            {
                var batchStatement = new BatchStatement();
                foreach (var o in batch)
                {
                    executer(batchStatement, o);
                }

                Connection.Session.Execute(batchStatement);
            }

        }
    }
}
