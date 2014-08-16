namespace StockGraphAnalyser.Domain.Web.Interfaces
{
    using System.Collections.Generic;

    public interface IYahooStockQuoteServiceClient
    {
        IEnumerable<Quote> GetQuotes(string ticker);
    }
}