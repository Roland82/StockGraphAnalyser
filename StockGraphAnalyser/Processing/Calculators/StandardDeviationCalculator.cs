
namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class StandardDeviationCalculator : ICalculationTool
    {
        private readonly Dictionary<DateTime, decimal> closingPrices;
        private const int StandardDeviationSampleSize = 20;

        public StandardDeviationCalculator(Dictionary<DateTime, decimal> closingPrices) {
            this.closingPrices = closingPrices;
        }

        public Task<Dictionary<DateTime, decimal>> Calculate()
        {
            return Task.Run(() =>
                                {
                                    Console.WriteLine("Getting Standard Deviation");
                                    var returnDictionary = new Dictionary<DateTime, decimal>();

                                    for (var i = 0; i <= this.closingPrices.Count() - StandardDeviationSampleSize; i++)
                                    {
                                        returnDictionary.Add(
                                            this.closingPrices.Keys.ElementAt(i + StandardDeviationSampleSize - 1),
                                            Math.Round(MathExtras.StandardDeviation(this.closingPrices.Skip(i).Take(StandardDeviationSampleSize).Select(e => e.Value)), 2)
                                            );
                                    }

                                    Console.WriteLine("Got Standard Deviation");
                                    return returnDictionary;
                                });
        }
    }
}
