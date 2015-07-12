

namespace StockGraphAnalyser.Actors
{
    using System;
    using System.Collections.Generic;
    using Akka.Actor;
    using StockGraphAnalyser.Actors.Children;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository.Cassandra;
    using StockGraphAnalyser.Domain.StockDataProviders;

    public class StockDataUpdaterActor : ReceiveActor
    {
        private IEnumerable<Quote> yahooQuotes;
  
        public StockDataUpdaterActor(String symbol) {
            var yahooStockDataActor = Context.ActorOf(Props.Create<YahooStockDataActor>(symbol, new YahooStockQuoteServiceClient()), "YahooStockDataActor_" + symbol);
            var localStockDataActor = Context.ActorOf(Props.Create<LocalStockDataActor>(symbol, new DataPointRepository()), "DataPointRepositoryActor_" + symbol);
            
            yahooStockDataActor.Tell("Go", Self);
            localStockDataActor.Tell("Go", Self);

            Receive<YahooStockDataActor.YahooDataMessage>(m => this.yahooQuotes = m.Quotes);

        }
    }
}
