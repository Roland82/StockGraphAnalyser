
namespace StockGraphAnalyser.Processing.Candlestick
{
    using System;
    using System.Collections.Generic;

    interface IDetectPattern
    {
        IEnumerable<DateTime> FindOccurences();
    }
}
