

namespace StockGraphAnalyser.Processing.Types
{
    using System;

    /// <summary>
    /// A holder for price details on a given day.
    /// </summary>
    public class Quote
    {
        private decimal? openToClosePercentageMovement;

        private Quote(string symbol, DateTime date, decimal open, decimal close, decimal high, decimal low, long volume)
        {
            this.Symbol = symbol;
            this.Date = date;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
        }

        public static Quote Create(string symbol, DateTime date, decimal open, decimal close, decimal high, decimal low, long volume)
        {
            return new Quote(symbol, date, open, close, high, low, volume);
        }

        public DateTime Date { get; private set; }
        public decimal Open { get; private set; }
        public decimal High { get; private set; }
        public decimal Low { get; private set; }
        public decimal Close { get; private set; }
        public string Symbol { get; set; }
        public long Volume { get; private set; }

        public decimal OpenToClosePercentageMovement {
            get {
                if (!this.openToClosePercentageMovement.HasValue)
                {
                    this.openToClosePercentageMovement = ((this.Close/this.Open) - 1) * 100;
                }

                return Math.Round(this.openToClosePercentageMovement.Value, 2);
            }
        }
    }
}
