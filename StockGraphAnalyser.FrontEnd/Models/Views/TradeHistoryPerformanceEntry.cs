
namespace StockGraphAnalyser.FrontEnd.Models.Views
{
    using System;
    using Domain;

    public class TradeHistoryPerformanceEntry
    {
        public DateTime DateTime { get; private set; }
        public decimal? CurrentEquity { get; private set; }
        public SignalType SignalType { get; private set; }
        public decimal PercentageChange { get; private set; }
        public decimal CurrentPrice { get; private set; }

        public TradeHistoryPerformanceEntry(DateTime dateTime, decimal? currentEquity, SignalType signalType, decimal percentageChange, decimal currentPrice) {
            this.DateTime = dateTime;
            this.CurrentEquity = currentEquity;
            this.SignalType = signalType;
            this.PercentageChange = percentageChange;
            this.CurrentPrice = currentPrice;
        }
    }
}