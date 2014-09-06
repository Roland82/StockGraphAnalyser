
namespace StockGraphAnalyser.Domain
{
    using System;

    public class DataPoints
    {
        private decimal? calculatedPercentageChange = null;

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
        
        public decimal? EmaTwentyTwoDay { get; set; }
        public decimal? EmaTwelveDay { get; set; }

        public decimal? MacdTwentyTwoOverTwelveDay { get; set; }
        public decimal? MacdTwentyTwoOverTwelveDaySignalLine { get; set; }
        public decimal? MacdTwentyTwoOverTwelveDayHistogram { get; set; }

        public decimal? UpperBollingerBandTwoDeviation { get; set; }
        public decimal? LowerBollingerBandTwoDeviation { get; set; }

        public decimal? UpperBollingerBandOneDeviation { get; set; }
        public decimal? LowerBollingerBandOneDeviation { get; set; }

        public decimal? ForceIndexOnePeriod { get; set; }
        public decimal? ForceIndexThirteenPeriod { get; set; }
        public short IsProcessed { get; set; }
        
        public decimal PercentageChange {
            get {               
                if (!this.calculatedPercentageChange.HasValue)
                {
                    this.calculatedPercentageChange = ((this.Close / this.Open) - 1) * 100;
                }

                return Math.Round(this.calculatedPercentageChange.Value, 2);
            }
        }

        protected bool Equals(DataPoints other)
        {
            return this.Id.Equals(other.Id) && this.Date.Equals(other.Date) && this.Open == other.Open && this.High == other.High && this.Low == other.Low && this.Close == other.Close && this.Volume == other.Volume && string.Equals(this.Symbol, other.Symbol) && this.MovingAverageTwoHundredDay == other.MovingAverageTwoHundredDay && this.MovingAverageFiftyDay == other.MovingAverageFiftyDay && this.MovingAverageTwentyDay == other.MovingAverageTwentyDay && this.EmaTwentyTwoDay == other.EmaTwentyTwoDay && this.EmaTwelveDay == other.EmaTwelveDay && this.MacdTwentyTwoOverTwelveDay == other.MacdTwentyTwoOverTwelveDay && this.UpperBollingerBandTwoDeviation == other.UpperBollingerBandTwoDeviation && this.LowerBollingerBandTwoDeviation == other.LowerBollingerBandTwoDeviation && this.UpperBollingerBandOneDeviation == other.UpperBollingerBandOneDeviation && this.LowerBollingerBandOneDeviation == other.LowerBollingerBandOneDeviation && this.ForceIndexOnePeriod == other.ForceIndexOnePeriod && this.ForceIndexThirteenPeriod == other.ForceIndexThirteenPeriod && this.IsProcessed == other.IsProcessed;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataPoints) obj);
        }

        public override int GetHashCode()
        {
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
                hashCode = (hashCode*397) ^ this.EmaTwentyTwoDay.GetHashCode();
                hashCode = (hashCode*397) ^ this.EmaTwelveDay.GetHashCode();
                hashCode = (hashCode*397) ^ this.MacdTwentyTwoOverTwelveDay.GetHashCode();
                hashCode = (hashCode*397) ^ this.UpperBollingerBandTwoDeviation.GetHashCode();
                hashCode = (hashCode*397) ^ this.LowerBollingerBandTwoDeviation.GetHashCode();
                hashCode = (hashCode*397) ^ this.UpperBollingerBandOneDeviation.GetHashCode();
                hashCode = (hashCode*397) ^ this.LowerBollingerBandOneDeviation.GetHashCode();
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
