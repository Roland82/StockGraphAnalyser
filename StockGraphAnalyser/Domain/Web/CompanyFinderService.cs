namespace StockGraphAnalyser.Domain.Web
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using HtmlAgilityPack;

    public class SymbolFinderService 
    {
        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Gets all symbols.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllSymbols()
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var symbolList = new Dictionary<string, string>();
            var document = new HtmlDocument();
            foreach (var letter in alphabet)
            {
                var responseStream = WebRequest.Create("http://uk.advfn.com/exchanges/LSE/" + letter).GetResponse().GetResponseStream();
                document.LoadHtml(new StreamReader(responseStream).ReadToEnd());
                var tickerRows = document.DocumentNode.SelectNodes("//tr[@class='even'or @class='odd' or @class='odd first']").Skip(1);
                tickerRows.ToList().ForEach(r => symbolList.Add(r.ChildNodes.ElementAt(1).InnerText, r.ChildNodes.ElementAt(0).InnerText));
            }

            return symbolList;
        }

        /// <summary>
        /// Gets all symbols.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetFtse100()
        {
            var symbolList = new Dictionary<string, string>();
            var document = new HtmlDocument();

            var responseStream = WebRequest.Create("http://en.wikipedia.org/wiki/FTSE_100_Index").GetResponse().GetResponseStream();
            document.LoadHtml(new StreamReader(responseStream).ReadToEnd());
            var tableRows = document.DocumentNode.SelectNodes("//table[@id='constituents']/tr");
            var tickerRows = tableRows.Skip(1).Take(tableRows.Count() - 1);
            foreach (var r in tickerRows)
            {
                var symbol = r.ChildNodes.Where(t => t.Name == "td").ElementAt(1).InnerText;
                var companyName = r.ChildNodes.Where(t => t.Name == "td").ElementAt(0).InnerText;

                if (!symbolList.ContainsKey(symbol))
                {
                    symbolList.Add(symbol, companyName);
                }
            }
          
            return symbolList;
        }
    }
}