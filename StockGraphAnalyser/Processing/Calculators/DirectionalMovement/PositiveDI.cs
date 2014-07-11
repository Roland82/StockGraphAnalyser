
namespace StockGraphAnalyser.Processing.Calculators.DirectionalMovement
{
    using System.Collections.Generic;
    using Types;

    public class PositiveDirectionalIndicator
    {
        private IEnumerable<Quote> quotes;

        public PositiveDirectionalIndicator(IEnumerable<Quote> quotes) {
            this.quotes = quotes;
        }
    }
}
