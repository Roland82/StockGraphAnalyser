
namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;  

    public class ForceIndexCalculator : ICalculationTool
    {
        private readonly IEnumerable<Tuple<DateTime, decimal, long>> data;

        public ForceIndexCalculator(IEnumerable<Tuple<DateTime, decimal, long>> data) {
            this.data = data;
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync() {
            return Task.Run(() => this.Process(DateTime.MinValue));
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync(DateTime fromDate) {
            return Task.Run(() => this.Process(fromDate));
        }

        private Dictionary<DateTime, decimal> Process(DateTime fromDate) {        
            var forceIndexData = new Dictionary<DateTime, decimal>();
            var safeDateToStartFrom = this.data.FirstOrDefault(d => d.Item1 >= fromDate);

            if (safeDateToStartFrom != null)
            {
                var orderedDataToProcess = this.data.OrderByDescending(t => t.Item1);
                var indexOfDate = orderedDataToProcess.IndexOf(t => t.Item1 == safeDateToStartFrom.Item1) + 1;

                for (var i = 0; i < indexOfDate; i++)
                {
                    if (i == orderedDataToProcess.Count() - 1) continue;

                    forceIndexData.Add(orderedDataToProcess.ElementAt(i).Item1,
                                       ((orderedDataToProcess.ElementAt(i).Item2 -
                                         orderedDataToProcess.ElementAt(i + 1).Item2)*
                                        orderedDataToProcess.ElementAt(i).Item3));
                }
            }

            return forceIndexData.OrderBy(t => t.Key).ToDictionary(d => d.Key, d => d.Value);
        }
    }
}
