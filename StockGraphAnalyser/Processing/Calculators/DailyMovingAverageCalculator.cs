

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class DailyMovingAverageCalculator : ICalculationTool
    {
        private readonly Dictionary<DateTime, decimal> closingPrices;
        private readonly int sampleSize;

        public DailyMovingAverageCalculator(Dictionary<DateTime, decimal> closingPrices, int sampleSize)
        {
            AssertArguments.GreaterThanOrEqualTo(closingPrices.Count(), sampleSize);
            this.closingPrices = closingPrices;
            this.sampleSize = sampleSize;
        }

        public Task<Dictionary<DateTime, decimal>> Calculate()
        {
            return Task.Run(() =>
                                {
                                    Console.WriteLine("Getting Moving average " + sampleSize);
                                    var movingAverages = new Dictionary<DateTime, decimal>();
                                    var limit = this.closingPrices.Count() - this.sampleSize;

                                    for (var x = 0; x <= limit; x++)
                                    {
                                        movingAverages.Add(
                                            this.closingPrices.ElementAt(x + sampleSize - 1).Key,
                                            this.closingPrices.Skip(x).Take(this.sampleSize).Average(q => q.Value)
                                            );
                                    }
                                    Console.WriteLine("Got Moving average " + sampleSize);
                                    return movingAverages;
                                });
        }
    }
}
