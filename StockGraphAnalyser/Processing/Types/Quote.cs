

namespace StockGraphAnalyser.Processing.Types
{
    using System;

    /// <summary>
    /// A holder for price details on a given day.
    /// </summary>
    public class Quote
    {
        private Quote(string symbol, DateTime date, decimal open, decimal close, decimal high, decimal low)
        {
            this.Symbol = symbol;
            this.Date = date;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
        }

        public static Quote Create(string symbol, DateTime date, decimal open, decimal close, decimal high, decimal low)
        {
            return new Quote(symbol, date, open, close, high, low);
        }

        public DateTime Date { get; private set; }
        public decimal Open { get; private set; }
        public decimal High { get; private set; }
        public decimal Low { get; private set; }
        public decimal Close { get; private set; }
        public string Symbol { get; set; }
    }
}
