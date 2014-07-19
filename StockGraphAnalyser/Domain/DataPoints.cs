
namespace StockGraphAnalyser.Domain
{
    using System;
    using Processing.Types;

    public class DataPoints
    {
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public decimal Open { get; private set; }
        public decimal High { get; private set; }
        public decimal Low { get; private set; }
        public decimal Close { get; private set; }
        public string Symbol { get; set; }
        public decimal? MovingAverageTwoHundredDay { get; set; }
        public decimal? MovingAverageFiftyDay { get; set; }
        public decimal? UpperBollingerBand { get; set; }
        public decimal? LowerBollingerBand { get; set; }
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
                    IsProcessed = false
                };
        }
    }
}
