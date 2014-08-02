
namespace StockGraphAnalyser.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICalculationTool
    {
        Task<Dictionary<DateTime, decimal>> Calculate();
        Task<Dictionary<DateTime, decimal>> Calculate(DateTime fromDate);
    }
}
