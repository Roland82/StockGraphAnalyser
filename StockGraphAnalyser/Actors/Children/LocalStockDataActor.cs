
namespace StockGraphAnalyser.Actors.Children
{
    using Akka.Actor;
    using StockGraphAnalyser.Domain.Repository.Cassandra;

    public class LocalStockDataActor : ReceiveActor
    {
        public LocalStockDataActor(string symbol, DataPointRepository repository) {
            ReceiveAny((o) => Sender.Tell(repository.FindAll(symbol)));
        }
    }
}
