

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;

    interface IGraphPlotter
    {
        Dictionary<DateTime, decimal> Calculate();
    }
}
