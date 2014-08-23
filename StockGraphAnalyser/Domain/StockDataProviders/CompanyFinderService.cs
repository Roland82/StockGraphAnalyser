namespace StockGraphAnalyser.Domain.StockDataProviders
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using HtmlAgilityPack;
    using Interfaces;

    public class CompanyFinderService : ICompanyFinderService
    {
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
        public Dictionary<string, string> GetFtseIndex(Company.ConstituentOfIndex indexType)
        {
            var symbolList = new Dictionary<string, string>();
            var document = new HtmlDocument();
            var urls = new Dictionary<Company.ConstituentOfIndex, string>();
            urls.Add(Company.ConstituentOfIndex.Ftse100, "http://www.lse.co.uk/index-constituents.asp?index=IDX:UKX&indexname=ftse_100");
            urls.Add(Company.ConstituentOfIndex.Ftse250, "http://www.lse.co.uk/index-constituents.asp?index=IDX:MCX&indexname=ftse_250");
            urls.Add(Company.ConstituentOfIndex.SmallCap, "http://www.lse.co.uk/index-constituents.asp?index=IDX:SMX&indexname=ftse_small_cap");
                                                                             

            var responseStream = WebRequest.Create(urls[indexType]).GetResponse().GetResponseStream();
            document.LoadHtml(new StreamReader(responseStream).ReadToEnd());
            var tableRows = document.DocumentNode.SelectNodes("//table[@class='redTable']/tr");
            var tickerRows = tableRows.Skip(1).Take(tableRows.Count() - 1);
            foreach (var r in tickerRows)
            {
                var companyNameAndSymbol = r.ChildNodes.Where(t => t.Name == "td").ElementAt(0).InnerText;

                var match = Regex.Match(companyNameAndSymbol, @"(?<company>.*) \((?<symbol>[A-Za-z0-9]*)\)");
                if (match.Success)
                {
                    symbolList.Add(match.Groups["symbol"].Value, match.Groups["company"].Value);
                }
            }
          
            return symbolList;
        }
    }
}