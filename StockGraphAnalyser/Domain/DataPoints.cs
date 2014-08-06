
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
        public decimal? MovingAverageTwentyDay { get; set; }
        public decimal? UpperBollingerBand { get; set; }
        public decimal? LowerBollingerBand { get; set; }
        public decimal? ForceIndexOnePeriod { get; set; }
        public decimal? ForceIndexThirteenPeriod { get; set; }
        public short IsProcessed { get; set; }

        protected bool Equals(DataPoints other) {
            return this.Id.Equals(other.Id) && this.Date.Equals(other.Date) && this.Open == other.Open && this.High == other.High && 
                   this.Low == other.Low && this.Close == other.Close && this.Volume == other.Volume && string.Equals(this.Symbol, other.Symbol) 
                   && this.MovingAverageTwoHundredDay == other.MovingAverageTwoHundredDay && this.MovingAverageFiftyDay == other.MovingAverageFiftyDay 
                   && this.MovingAverageTwentyDay == other.MovingAverageTwentyDay && this.UpperBollingerBand == other.UpperBollingerBand 
                   && this.LowerBollingerBand == other.LowerBollingerBand && this.ForceIndexOnePeriod == other.ForceIndexOnePeriod 
                   && this.ForceIndexThirteenPeriod == other.ForceIndexThirteenPeriod && this.IsProcessed == other.IsProcessed;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataPoints) obj);
        }

        public override int GetHashCode() {
            unchecked
            {
                int hashCode = this.Id.GetHashCode();
                hashCode = (hashCode*397) ^ this.Date.GetHashCode();
                hashCode = (hashCode*397) ^ this.Open.GetHashCode();
                hashCode = (hashCode*397) ^ this.High.GetHashCode();
                hashCode = (hashCode*397) ^ this.Low.GetHashCode();
                hashCode = (hashCode*397) ^ this.Close.GetHashCode();
                hashCode = (hashCode*397) ^ this.Volume.GetHashCode();
                hashCode = (hashCode*397) ^ (this.Symbol != null ? this.Symbol.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ this.MovingAverageTwoHundredDay.GetHashCode();
                hashCode = (hashCode*397) ^ this.MovingAverageFiftyDay.GetHashCode();
                hashCode = (hashCode*397) ^ this.MovingAverageTwentyDay.GetHashCode();
                hashCode = (hashCode*397) ^ this.UpperBollingerBand.GetHashCode();
                hashCode = (hashCode*397) ^ this.LowerBollingerBand.GetHashCode();
                hashCode = (hashCode*397) ^ this.ForceIndexOnePeriod.GetHashCode();
                hashCode = (hashCode*397) ^ this.ForceIndexThirteenPeriod.GetHashCode();
                hashCode = (hashCode*397) ^ this.IsProcessed.GetHashCode();
                return hashCode;
            }
        }

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
                    IsProcessed = 0
                };
        }
    }
}
