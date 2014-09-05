
namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using StockGraphAnalyser.Domain;

    public class DirectionalMovementCalculator : ICalculationTool
    {
        private readonly IEnumerable<DataPoints> dataPoints;
        private readonly Type directionalMovementType;

        public enum Type
        {
            Minus,
            Plus
        }

        public DirectionalMovementCalculator(IEnumerable<DataPoints> dataPoints, Type directionalMovementType) {
            this.dataPoints = dataPoints;
            this.directionalMovementType = directionalMovementType;
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync() {
            return Task.Run(() => this.Calculate(DateTime.MinValue));
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync(DateTime fromDate)
        {
            return Task.Run(() => this.Calculate(fromDate));
        }

        private Dictionary<DateTime, decimal> Calculate(DateTime fromDate)
        {
            var returnDictionary = new Dictionary<DateTime, decimal>();
            this.dataPoints.ForEachGroup(2, d =>
                {
                    var result = this.directionalMovementType == Type.Plus
                        ? PlusDirectionCalculation(d.ElementAt(0).High, d.ElementAt(1).High)
                        : MinusDirectionCalculation(d.ElementAt(0).Low, d.ElementAt(1).Low);
                    
                    returnDictionary.Add(d.ElementAt(1).Date, result);
                });

            return returnDictionary.Where(d => d.Key >= fromDate).ToDictionary(d => d.Key, d => d.Value);
        }

        private static decimal PlusDirectionCalculation(decimal yesterdaysHigh, decimal todaysHigh) {
            var result = todaysHigh - yesterdaysHigh;
            return result > 0 ? result : 0;
        }

        private static decimal MinusDirectionCalculation(decimal yesterdaysLow, decimal todaysLow)
        {
            var result = yesterdaysLow - todaysLow;
            return result > 0 ? Math.Abs(result) : 0;
        }
    }
}
