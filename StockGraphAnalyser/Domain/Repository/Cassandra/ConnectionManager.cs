

namespace StockGraphAnalyser.Domain.Repository.Cassandra
{
    using global::Cassandra;

    public class ConnectionManager : IConnectionManager
    {
        private static ConnectionManager instance;
        private readonly ISession session;

        private ConnectionManager(ISession session) {
            this.session = session;
        }

        public static ConnectionManager Instance {
            get {
                if (instance == null)
                {
                    var session = Cluster
                        .Builder()
                        .AddContactPoint("localhost")
                        .WithDefaultKeyspace("stockgraphanalyser")
                        .WithQueryOptions(new QueryOptions().SetConsistencyLevel(ConsistencyLevel.One))
                        .WithLoadBalancingPolicy(new TokenAwarePolicy(new DCAwareRoundRobinPolicy()))
                        .Build().Connect();
                    instance = new ConnectionManager(session);
                }

                return instance;
            }
        }

        public ISession Session {
            get { return this.session; }
        }
    }
}
