

namespace StockGraphAnalyser.Domain
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

        protected bool Equals(Quote other) {
            return 
                this.Date.Equals(other.Date) && this.Open == other.Open && this.High == other.High 
                && this.Low == other.Low && this.Close == other.Close 
                && string.Equals(this.Symbol, other.Symbol) && this.Volume == other.Volume;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((Quote) obj);
        }

        public override int GetHashCode() {
            unchecked
            {
                int hashCode = this.Date.GetHashCode();
                hashCode = (hashCode*397) ^ this.Open.GetHashCode();
                hashCode = (hashCode*397) ^ this.High.GetHashCode();
                hashCode = (hashCode*397) ^ this.Low.GetHashCode();
                hashCode = (hashCode*397) ^ this.Close.GetHashCode();
                hashCode = (hashCode*397) ^ (this.Symbol != null ? this.Symbol.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ this.Volume.GetHashCode();
                return hashCode;
            }
        }
    }
}
