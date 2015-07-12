
namespace StockGraphAnalyser.Domain.Repository.Cassandra
{
    using global::Cassandra;

    public interface IConnectionManager
    {
        ISession Session { get; }
    }
}
