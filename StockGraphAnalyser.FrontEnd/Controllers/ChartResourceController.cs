
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Mvc;
    using StockGraphAnalyser.FrontEnd.Infrastructure;

    public class ChartResourceController : ApiController
    {
        public ActionResult GetProduct() {
            var dictionary = new Dictionary<DateTime, decimal>
                {
                    {DateTime.Today, 1m},
                    {DateTime.Today.AddDays(1), 2m},
                    {DateTime.Today.AddDays(2), 3m},
                    {DateTime.Today.AddDays(3), 4m},
                    {DateTime.Today.AddDays(4), 5m}
                };

            var outputDictionary = dictionary.ToDictionary(e => e.Key.Ticks, e => e.Value);
            var list = new List<decimal[]>();
            foreach (var price in outputDictionary)
            {
                list.Add(new[] { price.Key, price.Value });
            }


            return JsonNetResult.Create(list);
        }
    }
}
