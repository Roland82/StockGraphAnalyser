

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DifferenceCalculator : ICalculationTool
    {
        private readonly Dictionary<DateTime, decimal> graphOne;
        private readonly Dictionary<DateTime, decimal> graphTwo;

        public DifferenceCalculator(Dictionary<DateTime, decimal> graphOne, Dictionary<DateTime, decimal> graphTwo) {
            this.graphOne = graphOne;
            this.graphTwo = graphTwo;
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync()
        {
            return Task.Run(() => this.Process(DateTime.MinValue));
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync(DateTime fromDate) {
            return Task.Run(() => Process(fromDate));
        }

        private Dictionary<DateTime, decimal> Process(DateTime fromDate)
        {
            var sharedRange = Utilities.CalculateSharedDateRange(
                this.graphOne.Select(s => s.Key), 
                this.graphTwo.Select(s => s.Key));

            var slowMa = this.graphTwo.SubsetBetween(sharedRange.Item1, sharedRange.Item2).Where(d => d.Key >= fromDate);
            var fastMa = this.graphOne.SubsetBetween(sharedRange.Item1, sharedRange.Item2).Where(d => d.Key >= fromDate);
            var histogram = new Dictionary<DateTime, decimal>();
            for (var i = 0; i < slowMa.Count(); i++)
            {
                histogram.Add(slowMa.ElementAt(i).Key, fastMa.ElementAt(i).Value - slowMa.ElementAt(i).Value);
            }

            return histogram;
        } 
    }
}
