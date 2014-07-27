
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Mvc;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository;
    using StockGraphAnalyser.FrontEnd.Infrastructure;

    public class ChartResourceController : ApiController
    {
        public ActionResult GetInidcator(string symbol, string indicatorName = "Close")
        {
            var indicatorMap = new Dictionary<string, Func<DataPoints, decimal?>>{
                                                                                    { "Close", points => points.Close },
                                                                                    { "MovingAverageTwoHundredDay", points => points.MovingAverageTwoHundredDay},
                                                                                    { "MovingAverageFiftyDay", points => points.MovingAverageFiftyDay}
                                                                                };

            var repository = new DataPointRepository();
            var dataPointToUse = indicatorMap[indicatorName];
            var datapoints = repository.FindAll(symbol);           
            var outputDictionary = datapoints.ToDictionary(e => e.Date, dataPointToUse);
            var list = outputDictionary.Select(price => new object[] {price.Key.ToEpoch(), price.Value}).ToList();


            return JsonNetResult.Create(list);
        }

        public ActionResult GetCandleSticks(string ticker)
        {
            var repository = new DataPointRepository();
            var datapoints = repository.FindAll(ticker);
            return JsonNetResult.Create(datapoints.Select(d => new object[] { d.Date.ToEpoch(), d.Open, d.High, d.Low, d.Close }));
        }
    }
}
