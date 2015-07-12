
namespace StockGraphAnalyser.Actors.Children
{
    using System.Collections.Generic;
    using Akka.Actor;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.StockDataProviders;

    public class YahooStockDataActor : ReceiveActor
    {
        public YahooStockDataActor(string symbol, YahooStockQuoteServiceClient yahooStockQuoteService) {
            ReceiveAny((o) => Sender.Tell(new YahooDataMessage(yahooStockQuoteService.GetQuotes(symbol))));
        }

        public class YahooDataMessage
        {
            private readonly IEnumerable<Quote> quotes;

            public YahooDataMessage(IEnumerable<Quote> quotes) {
                this.quotes = quotes;
            }

            public IEnumerable<Quote> Quotes {
                get { return this.quotes; }
            }
        }
    }
}
