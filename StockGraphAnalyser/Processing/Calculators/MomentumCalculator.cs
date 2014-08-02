

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MomentumCalculator : ICalculationTool
    {
        private readonly Dictionary<DateTime, decimal> closingPrices;
        private readonly int periodGap;

        public MomentumCalculator(Dictionary<DateTime, decimal> closingPrices, int periodGap) {
            this.closingPrices = closingPrices;
            this.periodGap = periodGap;
            
            if (closingPrices.Count <= this.periodGap)
            {
                throw new ArgumentException("The period gap given exceeds the amount of prices passed in.");
            }
        }

        public Task<Dictionary<DateTime, decimal>> Calculate() {
            return Task.Run(() =>
                {
                    var momentum = new Dictionary<DateTime, decimal>();

                    for (var i = 0; i < this.closingPrices.Count - this.periodGap; i++)
                    {
                        var calculatedMomentum = this.closingPrices.ElementAt(i + periodGap).Value/
                                                 this.closingPrices.ElementAt(i).Value;
                        momentum.Add(this.closingPrices.ElementAt(i + periodGap).Key,
                                     Math.Round(calculatedMomentum*100m, 1));
                    }

                    return momentum;
                });
        }

        public Task<Dictionary<DateTime, decimal>> Calculate(DateTime fromDate) {
            throw new NotImplementedException();
        }
    }
}
