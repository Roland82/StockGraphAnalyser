

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExponentialMovingAverageCalculator : ICalculationTool
    {
        private readonly int timePeriod;
        private readonly IReadOnlyDictionary<DateTime, decimal> closingPrices;

        public ExponentialMovingAverageCalculator(IReadOnlyDictionary<DateTime, decimal> closingPrices, int timePeriod)
        {
            this.timePeriod = timePeriod;
            this.closingPrices = closingPrices;
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync() {

            return Task.Run(() => this.Process(DateTime.MinValue));
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync(DateTime fromDate) {
            return Task.Run(() => this.Process(fromDate));
        }

        private Dictionary<DateTime, decimal> Process(DateTime fromDate)
        {
            /* 
             * Unfortunately because EMA's rely on previous ema processed data and the first EMA is aways the SMA 
             * we always have to process the entire data set every time so it remains consistent for partial updates
             */

            var orderedClosingPrices = this.closingPrices.OrderBy(p => p.Key);
            var startingEmaValue = Math.Round(orderedClosingPrices.Take(this.timePeriod).Average(e => e.Value), 2);
            var startingEmaDate = orderedClosingPrices.Skip(timePeriod - 1).First().Key;

            var multiplier = (decimal)2 / (decimal)(this.timePeriod + 1);
            var ema = new Dictionary<DateTime, decimal> { { startingEmaDate, startingEmaValue } };

            var closingPricesToWorkWith = this.closingPrices.Where(e => e.Key > startingEmaDate).OrderBy(e => e.Key);

            foreach (var closingPrice in closingPricesToWorkWith)
            {
                var yesterdayEma = ema.LastOrDefault();
                ema.Add(closingPrice.Key, Math.Round(((closingPrice.Value - yesterdayEma.Value) * multiplier + yesterdayEma.Value), 2));
            }

            return ema.Where(d => d.Key >= fromDate).ToDictionary(d => d.Key, d => d.Value);
        } 
    }
}
