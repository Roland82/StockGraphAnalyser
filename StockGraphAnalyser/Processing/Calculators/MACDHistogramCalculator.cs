

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MacdHistogramCalculator : ICalculationTool
    {
        private readonly Dictionary<DateTime, decimal> fastMovingAverage;
        private readonly Dictionary<DateTime, decimal> slowMovingAverage;

        public MacdHistogramCalculator(Dictionary<DateTime, decimal> fastMovingAverage, Dictionary<DateTime, decimal> slowMovingAverage) {
            this.fastMovingAverage = fastMovingAverage;
            this.slowMovingAverage = slowMovingAverage;
        }

        public Task<Dictionary<DateTime, decimal>> Calculate()
        {
            return Task.Run(() =>
                                {
                                    var sharedRange = Utilities.CalculateSharedDateRange(this.fastMovingAverage.Select(s => s.Key), this.slowMovingAverage.Select(s => s.Key));
                                    var slowMa = this.slowMovingAverage.SubsetBetween(sharedRange.Item1, sharedRange.Item2);
                                    var fastMa = this.fastMovingAverage.SubsetBetween(sharedRange.Item1, sharedRange.Item2);
                                    var histogram = new Dictionary<DateTime, decimal>();
                                    for (var i = 0; i < slowMa.Count; i++)
                                    {
                                        histogram.Add(slowMa.ElementAt(i).Key, slowMa.ElementAt(i).Value - fastMa.ElementAt(i).Value);
                                    }

                                    return histogram;
                                });
        }

        public Task<Dictionary<DateTime, decimal>> Calculate(DateTime fromDate) {
            throw new NotImplementedException();
        }
    }
}
