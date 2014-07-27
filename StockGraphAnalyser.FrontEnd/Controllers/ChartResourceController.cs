
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
        public ActionResult GetProduct(string symbol, string indicatorName = "")
        {
            var indicatorMap = new Dictionary<string, Func<DataPoints, decimal?>>{
                                                                                    { "", points => points.Close },
                                                                                    { "MovingAverageTwoHundredDay", points => points.MovingAverageTwoHundredDay}
                                                                                };

            var repository = new DataPointRepository();
            var dataPointToUse = indicatorMap[indicatorName];
            var datapoints = repository.FindAll(symbol);           
            var outputDictionary = datapoints.ToDictionary(e => e.Date, dataPointToUse);
            var list = outputDictionary.Select(price => new object[] {price.Key.ToEpoch(), price.Value}).ToList();


            return JsonNetResult.Create(list);
        }
    }
}
