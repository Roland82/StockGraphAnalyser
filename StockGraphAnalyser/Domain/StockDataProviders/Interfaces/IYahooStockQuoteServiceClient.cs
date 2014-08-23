namespace StockGraphAnalyser.Domain.StockDataProviders.Interfaces
{
    using System.Collections.Generic;

    public interface IYahooStockQuoteServiceClient
    {
        IEnumerable<Quote> GetQuotes(string ticker);
    }
}