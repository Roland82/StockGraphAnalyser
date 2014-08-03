
namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICalculationTool
    {
        Task<Dictionary<DateTime, decimal>> CalculateAsync();
        Task<Dictionary<DateTime, decimal>> CalculateAsync(DateTime fromDate);
    }
}
