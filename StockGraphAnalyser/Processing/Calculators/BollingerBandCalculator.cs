namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class BollingerBandCalculator : ICalculationTool
    {
        public enum Band
        {
            Upper = 2,
            Lower = -2
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

        public Task<Dictionary<DateTime, decimal>> Calculate()
        {
            return Task.Run(() =>
                                {
                                    Console.WriteLine("Getting Bollinger Band " + this.band);
                                    var result = this.ma20Day.ToDictionary(m => m.Key, m => (this.standardDeviation[m.Key]*(decimal) this.band) + m.Value);
                                    Console.WriteLine("Got bollinger band");
                                    return result;
                                });
        }
    }
}