

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MacdHistogramCalculator : ICalculationTool
    {
        private readonly Dictionary<DateTime, decimal> fastExponentialMovingAverage;
        private readonly Dictionary<DateTime, decimal> slowExponentialMovingAverage;

        public MacdHistogramCalculator(Dictionary<DateTime, decimal> fastExponentialMovingAverage, Dictionary<DateTime, decimal> slowExponentialMovingAverage) {
            this.fastExponentialMovingAverage = fastExponentialMovingAverage;
            this.slowExponentialMovingAverage = slowExponentialMovingAverage;
        }

        public Task<Dictionary<DateTime, decimal>> Calculate()
        {
            return Task.Run(() => this.Process(DateTime.MinValue));
        }

        public Task<Dictionary<DateTime, decimal>> Calculate(DateTime fromDate) {
            throw new NotImplementedException();
        }

        private Dictionary<DateTime, decimal> Process(DateTime fromDate)
        {
            var sharedRange = Utilities.CalculateSharedDateRange(this.fastExponentialMovingAverage.Select(s => s.Key), this.slowExponentialMovingAverage.Select(s => s.Key));
            var slowMa = this.slowExponentialMovingAverage.SubsetBetween(sharedRange.Item1, sharedRange.Item2);
            var fastMa = this.fastExponentialMovingAverage.SubsetBetween(sharedRange.Item1, sharedRange.Item2);
            var histogram = new Dictionary<DateTime, decimal>();
            for (var i = 0; i < slowMa.Count; i++)
            {
                histogram.Add(slowMa.ElementAt(i).Key, slowMa.ElementAt(i).Value - fastMa.ElementAt(i).Value);
            }

            return histogram;
        } 
    }
}
