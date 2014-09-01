
namespace StockGraphAnalyser.Processing.Candlestick
{
    using System;
    using System.Collections.Generic;

    public interface IDetectPattern
    {
        int PatternType { get; }
        IEnumerable<DateTime> FindOccurences();
    }
}
