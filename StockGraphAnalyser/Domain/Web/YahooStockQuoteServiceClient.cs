

namespace StockGraphAnalyser.Domain.Web
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using CsvHelper;
    using Interfaces;
    using Processing.Types;

    public class YahooStockQuoteServiceClient : IYahooStockQuoteServiceClient
    {
        public IEnumerable<Quote> GetQuotes(string ticker) {
            var url = string.Format("http://ichart.finance.yahoo.com/table.csv?s={0}.L&ignore=.csv", ticker);
            try
            {
                var responseStream = WebRequest.Create(url).GetResponse().GetResponseStream();
                var returnList = new List<Quote>();

                if (responseStream == null) return returnList;

                var reader = new CsvReader(new StreamReader(responseStream));
                while (reader.Read())
                {
                    returnList.Add(Quote.Create(ticker + ".L",
                                                reader.GetField<DateTime>("Date"),
                                                reader.GetField<decimal>("Open"),
                                                reader.GetField<decimal>("Close"),
                                                reader.GetField<decimal>("High"),
                                                reader.GetField<decimal>("Low")
                                       ));
                }

                return returnList.OrderBy(q => q.Date).ToList();
            }
            catch (WebException e)
            {
                Console.Write("Cannot get quotes for " + ticker);
                return new List<Quote>();
            }
        }

    }
}
