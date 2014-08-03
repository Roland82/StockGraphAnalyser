﻿
namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class StandardDeviationCalculator : ICalculationTool
    {
        private readonly Dictionary<DateTime, decimal> closingPrices;
        private readonly int sampleSize;

        public StandardDeviationCalculator(Dictionary<DateTime, decimal> closingPrices, int sampleSize) {
            this.closingPrices = closingPrices;
            this.sampleSize = sampleSize;
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
            var returnDictionary = new Dictionary<DateTime, decimal>();

            for (var i = 0; i <= this.closingPrices.Count() - this.sampleSize; i++)
            {
                returnDictionary.Add(
                    this.closingPrices.Keys.ElementAt(i + this.sampleSize - 1),
                    Math.Round(MathExtras.StandardDeviation(this.closingPrices.Skip(i).Take(this.sampleSize).Select(e => e.Value)), 2)
                    );
            }

            return returnDictionary;
        } 
    }
}
