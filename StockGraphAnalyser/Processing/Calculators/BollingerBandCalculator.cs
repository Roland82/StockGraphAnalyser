namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BollingerBandCalculator : ICalculationTool
    {
        public enum Band
        {
            UpperTwoDeviation = 2,
            LowerTwoDeviation = -2,
            UpperOneDeviation = 1,
            LowerOneDeviation = -1
        }

        private readonly Band band;
        private readonly Dictionary<DateTime, decimal> ma20Day;
        private readonly Dictionary<DateTime, decimal> standardDeviation;

        public BollingerBandCalculator(Dictionary<DateTime, decimal> ma20Day, Dictionary<DateTime, decimal> standardDeviation, Band band)
        {
            this.ma20Day = ma20Day;
            this.standardDeviation = standardDeviation;
            this.band = band;
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync()
        {
            return Task.Run(() => this.Process(DateTime.MinValue));
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync(DateTime fromDate) {
            return Task.Run(() => this.Process(fromDate));
        }

        private Dictionary<DateTime, decimal> Process(DateTime fromDate) {
            var result = this.ma20Day
                .Where(e => e.Key >= fromDate)
                .ToDictionary(m => m.Key, 
                              m => Math.Round(this.standardDeviation[m.Key] * (decimal)this.band, 2) + m.Value);
            return result;
        } 
    }
}