namespace StockGraphAnalyser.Signals
{
    using System;
    using System.Collections.Generic;

    public interface ISignalEquityPositionTotaller
    {
        Dictionary<DateTime, decimal> Calculate();
    }
}