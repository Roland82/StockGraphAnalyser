
namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using StockGraphAnalyser.Domain;

    public class TrueRangeCalculator : ICalculationTool
    {
        private readonly IEnumerable<DataPoints> dataPointsList;

        public TrueRangeCalculator(IEnumerable<DataPoints> dataPointsList)
        {
            this.dataPointsList = dataPointsList;
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync()
        {
            return Task.Run(() => this.Calculate(DateTime.MinValue));
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync(DateTime fromDate)
        {
            return Task.Run(() => this.Calculate(fromDate));
        }

        private Dictionary<DateTime, decimal> Calculate(DateTime fromDate)
        {
            DataPoints lastDatapoints = null;
            var ranges = new Dictionary<DateTime, decimal>();

            foreach (var datapoints in this.dataPointsList)
            {
                if (lastDatapoints == null)
                {
                    lastDatapoints = datapoints;
                    continue;
                }

                var possibleRanges = new[]
                    {
                        Math.Abs(datapoints.High - lastDatapoints.Close),
                        Math.Abs(lastDatapoints.Close - datapoints.Low),
                        datapoints.High.Difference(datapoints.Low),
                    };

                ranges.Add(datapoints.Date, possibleRanges.Max());
                lastDatapoints = datapoints;
            }

            return ranges;
        } 
    }
}
