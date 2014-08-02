
namespace StockGraphAnalyser.Domain
{
    using System;
    using Processing.Types;

    public class DataPoints
    {
        public Guid Id { get; private set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public string Symbol { get; set; }
        public decimal? MovingAverageTwoHundredDay { get; set; }
        public decimal? MovingAverageFiftyDay { get; set; }
        public decimal? UpperBollingerBand { get; set; }
        public decimal? LowerBollingerBand { get; set; }
        public decimal? ForceIndexOnePeriod { get; set; }
        public decimal? ForceIndexThirteenPeriod { get; set; }
        public bool IsProcessed { get; set; }

        public static DataPoints CreateFromQuote(Quote quote)
        {
            return new DataPoints
                {
                    Id = Guid.NewGuid(), 
                    Date = quote.Date, 
                    Close = quote.Close, 
                    High = quote.High, 
                    Low = quote.Low, 
                    Open = quote.Open, 
                    Symbol = quote.Symbol, 
                    Volume = quote.Volume,
                    IsProcessed = false
                };
        }
    }
}
