
namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using StockGraphAnalyser.Domain;

    public class DirectionalMovementCalculator : ICalculationTool
    {
        private IEnumerable<DataPoints> dataPoints;

        public DirectionalMovementCalculator(IEnumerable<DataPoints> dataPoints)
        {
            this.dataPoints = dataPoints;
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync(DateTime fromDate)
        {
            throw new NotImplementedException();
        }

        private Dictionary<DateTime, decimal> Calculate()
        {
            throw new NotImplementedException();
        }
    }
}
