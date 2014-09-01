

namespace StockGraphAnalyser.FrontEnd.Models.Views
{
    using System.Collections.Generic;
    using Domain;

    public class TechnicalsViewModel
    {
        public string Ticker { get; private set; }
        public IEnumerable<CandleStickSignal> CandleStickSignals { get; private set; }

        public TechnicalsViewModel(string ticker, IEnumerable<CandleStickSignal> candleStickSignals) {
            this.Ticker = ticker;
            this.CandleStickSignals = candleStickSignals;
        }
    }
}