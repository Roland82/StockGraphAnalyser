
namespace StockGraphAnalyser.Processing.Candlestick
{
    using System;

    interface IDetectPattern
    {
        DateTime? LatestOccurence();
    }
}
