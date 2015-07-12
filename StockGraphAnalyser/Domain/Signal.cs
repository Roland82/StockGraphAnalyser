
namespace StockGraphAnalyser.Domain
{
    using System;

    public class Signal
    {
        public Guid Id { get; private set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public SignalType SignalType { get; set; }
        public decimal Price { get; set; }
        public decimal? StopLoss { get; set; }
        public decimal? CurrentEquity { get; set; }

        public Signal() { /* For dapper only */ }

        public static Signal Create(string symbol, DateTime date, SignalType signalType, decimal price, decimal? stopLoss = null)
        {
            return new Signal {
                                 Id = Guid.NewGuid(),
                                 Symbol = symbol,
                                 Date = date,
                                 SignalType = signalType,
                                 Price = price,
                                 StopLoss = stopLoss
                              };
        }
    }
}
