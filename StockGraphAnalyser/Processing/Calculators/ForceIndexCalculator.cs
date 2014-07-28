
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

        public Task<Dictionary<DateTime, decimal>> Calculate() {
            return Task.Run(() =>
                {
                    var forceIndexData = new Dictionary<DateTime, decimal>();

                    for (var i = 0; i < data.Count() - 1; i++)
                    {
                        forceIndexData.Add(data.ElementAt(i + 1).Item1, 
                        ((data.ElementAt(i).Item2 - data.ElementAt(i + 1).Item2) * data.ElementAt(i + 1).Item3) * -1);    
                    }

                    return forceIndexData;
                });
        }
    }
}
