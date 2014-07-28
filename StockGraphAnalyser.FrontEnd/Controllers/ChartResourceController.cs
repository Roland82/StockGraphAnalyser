
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class ChartResourceController : ApiController
    {
        public HttpResponseMessage GetProduct() {
            var dictionary = new Dictionary<DateTime, decimal>
                {
                    {DateTime.Today, 1m},
                    {DateTime.Today.AddDays(1), 2m},
                    {DateTime.Today.AddDays(2), 3m},
                    {DateTime.Today.AddDays(3), 4m},
                    {DateTime.Today.AddDays(4), 5m}
                };

            var outputDictionary = dictionary.ToDictionary(e => e.Key.Ticks, e => e.Value);
           
            return Request.CreateResponse(HttpStatusCode.OK, outputDictionary.ToArray());
        }
    }
}
