namespace StockGraphAnalyser
{
    using System;
    using System.Linq;
    using Domain.Web;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Domain.Repository;
    using StockGraphAnalyser.Processing;
    using StockGraphAnalyser.Processing.Calculators;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var letters = "GHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            foreach (var letter in letters)
            {
                var repo = new CompanyRepository();
                var companies = repo.FindAll(letter);

                foreach (var company in companies)
                {
                    Console.WriteLine("On" + company.Name);

                    var yahooService = new YahooStockQuoteServiceClient();
                    var quotes = yahooService.GetQuotes(company.Symbol).Select(DataPoints.CreateFromQuote).Where(q => q.Date > DateTime.Today.AddDays(-365));

                    if (quotes.Count() > 200)
                    {

                        var r = new DataPointRepository();
                        r.InsertAll(quotes);
                        var dataPoints = r.FindAll(company.Symbol + ".L");
                        var twoHundredDayMa = new DailyMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 200);
                        var fiftyDayMa = new DailyMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 50);
                        var twentyDayMa = new DailyMovingAverageCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close), 20);
                        var standardDeviationCalculator = new StandardDeviationCalculator(dataPoints.ToDictionary(q => q.Date, q => q.Close));


                        // Bollinger bands         
                        var twentyDayMaCalc = twentyDayMa.Calculate();
                        var twoHundredDayMaTask = twoHundredDayMa.Calculate();
                        var fiftyDayMaTask = fiftyDayMa.Calculate();
                        var standardDeviationTask = standardDeviationCalculator.Calculate();
                        var twentyDayMaResult = twentyDayMaCalc.Result;
                        var standardDeviationResult = standardDeviationTask.Result;


                        var upperBollingerBandTask = new BollingerBandCalculator(twentyDayMaResult, standardDeviationResult, BollingerBandCalculator.Band.Upper).Calculate();
                        var lowerBollingerBandTask = new BollingerBandCalculator(twentyDayMaResult, standardDeviationResult, BollingerBandCalculator.Band.Lower).Calculate();


                        dataPoints = dataPoints.MapNewDataPoint(twoHundredDayMaTask.Result, (p, d) => p.MovingAverageTwoHundredDay = d);
                        dataPoints = dataPoints.MapNewDataPoint(fiftyDayMaTask.Result, (p, d) => p.MovingAverageFiftyDay = d);
                        dataPoints = dataPoints.MapNewDataPoint(lowerBollingerBandTask.Result, (p, d) => p.LowerBollingerBand = d);
                        dataPoints = dataPoints.MapNewDataPoint(upperBollingerBandTask.Result, (p, d) => p.UpperBollingerBand = d);
                        dataPoints = dataPoints.UpdateAll(x => x.IsProcessed = true);
                        r.UpdateAll(dataPoints);
                    }
                }
            }
        }
    }
}