

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DailyMovingAverageCalculator : ICalculationTool
    {
        private readonly Dictionary<DateTime, decimal> closingPrices;
        private readonly int sampleSize;

        public DailyMovingAverageCalculator(Dictionary<DateTime, decimal> closingPrices, int sampleSize)
        {
            this.closingPrices = closingPrices;
            this.sampleSize = sampleSize;
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync()
        {
            return Task.Run(() => this.Process(DateTime.MinValue));
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync(DateTime fromDate) {
            return Task.Run(() => this.Process(fromDate));
        }

        private Dictionary<DateTime, decimal> Process(DateTime fromDate)
        {
            var reverseClosingPrices = this.closingPrices.OrderByDescending(c => c.Key).ToDictionary(e => e.Key, e => e.Value);     
   
            var limit = this.closingPrices.Count - this.closingPrices.Count(e => e.Key < fromDate);

            var movingAverage = new Dictionary<DateTime, decimal>();
            for (var i = 0; i < limit; i++)
            {
                var historicalPricesAvailable = reverseClosingPrices.Skip(i).Take(sampleSize);
                if (historicalPricesAvailable.Count() < sampleSize) break;
                movingAverage.Add(reverseClosingPrices.ElementAt(i).Key, historicalPricesAvailable.Average(e => e.Value));
            }

            return movingAverage.OrderBy(e => e.Key).ToDictionary(d => d.Key, d => d.Value);
        } 
    }
}
