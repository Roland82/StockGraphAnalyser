

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;

    public class ExponentialMovingAverageCalculator :ICalculationTool
    {
        private readonly int sampleSize;
        private Dictionary<DateTime, decimal> datapoints;

        public ExponentialMovingAverageCalculator(int sampleSize, Dictionary<DateTime, decimal> datapoints) {
            this.sampleSize = sampleSize;
            this.datapoints = datapoints;
        }


        public Task<Dictionary<DateTime, decimal>> Calculate() {
         
            //var multiplier = 2 / (this.sampleSize + 1);
            //var ema = new Dictionary<DateTime, decimal>();

            //foreach (var datapoint in this.datapoints)
            //{
            //    var yesterdayEma = ema.LastOrDefault();
            //    datapoint.Value * multiplier + (yesterdayEma == KeyValuePair. ? 0 : yesterdayEma.Value) * (1 – multiplier);
            //}

            //return 

            return null;
        }
    }
}
