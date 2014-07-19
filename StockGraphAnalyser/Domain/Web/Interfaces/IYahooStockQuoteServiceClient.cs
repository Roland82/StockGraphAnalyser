namespace StockGraphAnalyser.Domain.Web.Interfaces
{
    using System.Collections.Generic;
    using Processing.Types;

    public interface IYahooStockQuoteServiceClient
    {
        IEnumerable<Quote> GetQuotes(string ticker);
    }
}