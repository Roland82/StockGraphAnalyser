﻿
namespace StockGraphAnalyser.FrontEnd.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Mvc;
    using Domain;
    using Domain.Repository;
    using Infrastructure;

    public class ChartResourceController : ApiController
    {
        public ActionResult GetInidcator(string symbol, string indicatorName)
        {
            var indicatorMap = new Dictionary<string, Func<DataPoints, decimal?>>{
                                                                                    { "MovingAverageTwoHundredDay", points => points.MovingAverageTwoHundredDay},
                                                                                    { "MovingAverageFiftyDay", points => points.MovingAverageFiftyDay},
                                                                                    { "UpperBollingerBandTwoDeviation", points => points.UpperBollingerBandTwoDeviation},
                                                                                    { "LowerBollingerBandTwoDeviation", points => points.LowerBollingerBandTwoDeviation},
                                                                                    { "Volume", points => points.Volume}
                                                                                };

            var repository = new DataPointRepository();
            var dataPointToUse = indicatorMap[indicatorName];
            var datapoints = repository.FindAll(symbol);           
            var list = datapoints.Select(d => new object[] { d.Date.ToEpoch(), dataPointToUse(d) }).ToList();
            return JsonNetResult.Create(list);
        }

        public ActionResult Get(string symbol)
        {
            var repository = new DataPointRepository();
            var datapoints = repository.FindAll(symbol);
            var list = datapoints.Select(d => new []
                {
                    d.Date.ToEpoch(), d.Open, d.High, d.Low, d.Close, d.Volume, 
                    d.ForceIndexThirteenPeriod, 
                    d.UpperBollingerBandTwoDeviation, d.LowerBollingerBandTwoDeviation,
                    d.MacdTwentyTwoOverTwelveDay, d.MacdTwentyTwoOverTwelveDaySignalLine, d.MacdTwentyTwoOverTwelveDayHistogram, d.MovingAverageTwentyDay
                }).ToList();

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
