
namespace StockGraphAnalyser.Domain
{
    using System;

    public class CandleStickSignal
    {
        public Guid Id;
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public int CandleStickSignalType { get; set; }

        private CandleStickSignal(string symbol, DateTime date, int candleStickSignalType) {
            this.Id = Guid.NewGuid();
            this.Symbol = symbol;
            this.Date = date;
            this.CandleStickSignalType = candleStickSignalType;
        }

        public static CandleStickSignal Create(string symbol, DateTime date, int candleStickSignalType) {
            return new CandleStickSignal(symbol, date, candleStickSignalType);
        }
    }
}
