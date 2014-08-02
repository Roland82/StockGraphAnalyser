

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExponentialMovingAverageCalculator : ICalculationTool
    {
        private readonly int timePeriod;
        private readonly Dictionary<DateTime, decimal> closingPrices;

        public ExponentialMovingAverageCalculator(Dictionary<DateTime, decimal> closingPrices, int timePeriod) {
            this.timePeriod = timePeriod;
            this.closingPrices = closingPrices;
        }

        public Task<Dictionary<DateTime, decimal>> Calculate() {

            return Task.Run(() =>
                {
                    var orderedClosingPrices = this.closingPrices.OrderBy(p => p.Key);
                    var startingEmaValue = Math.Round(orderedClosingPrices.Take(this.timePeriod - 1).Average(e => e.Value), 2);
                    var startingEmaDate = orderedClosingPrices.Skip(timePeriod).First().Key;

                    var multiplier = (decimal)2 / (decimal)(this.timePeriod + 1);
                    var ema = new Dictionary<DateTime, decimal> { { startingEmaDate, startingEmaValue } };

                    var closingPricesToWorkWith =
                        this.closingPrices.Where(e => e.Key > startingEmaDate).OrderBy(e => e.Key);

                    foreach (var closingPrice in closingPricesToWorkWith)
                    {
                        var yesterdayEma = ema.LastOrDefault();
                        ema.Add(closingPrice.Key, Math.Round(((closingPrice.Value - yesterdayEma.Value)*multiplier + yesterdayEma.Value), 2));
                    }

                    return ema;
                });
        }

        public Task<Dictionary<DateTime, decimal>> Calculate(DateTime fromDate) {
            throw new NotImplementedException();
        }
    }
}
