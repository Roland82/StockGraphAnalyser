

namespace StockGraphAnalyser.Processing.Calculators.TradingSignalDetectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    public class OverboughtInDowntrendSignal
    {
        private IEnumerable<DataPoints> datapoints;

        public OverboughtInDowntrendSignal(IEnumerable<DataPoints> datapoints) {
            this.datapoints = datapoints;
        }

        public DateTime? FindLatestSignal() {
            if (this.IsStockInDowntrend())
            {
                var lastOccurence = this.datapoints.OrderByDescending(d => d.Date).Where(IsClosingPriceOverbought).LastOrDefault();
                if (lastOccurence != null)
                {
                    return lastOccurence.Date;
                }
            }

            return null;
        }

        private bool IsClosingPriceOverbought(DataPoints datapoints) {
            return datapoints.Close > datapoints.UpperBollingerBand &&
                   datapoints.Close > datapoints.MovingAverageFiftyDay;
        }

        private bool IsStockInDowntrend() {
            var count = datapoints.Count();
            return this.datapoints.ElementAt(count - 1).MovingAverageFiftyDay >
                        this.datapoints.ElementAt(count - 2).MovingAverageTwoHundredDay;
        }
    }
}
