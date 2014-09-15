

namespace StockGraphAnalyser.FrontEnd.Models.Views
{
    using Domain;

    public class TechnicalsViewModel
    {
        public string Ticker { get; private set; }
        public Company Company { get; private set; }
        public decimal TradingPerformancePercentage { get; private set; }

        public TechnicalsViewModel(string ticker, Company company, decimal tradingPerformancePercentage) {
            this.Ticker = ticker;
            this.Company = company;
            this.TradingPerformancePercentage = tradingPerformancePercentage;
        }
    }
}