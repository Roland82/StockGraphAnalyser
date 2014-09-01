namespace StockGraphAnalyser.Processing.Candlestick
{
    using System.Collections.Generic;
    using Domain;

    public interface ICandlestickPatternRecogniserFactory
    {
        IEnumerable<IDetectPattern> CreateAll(IEnumerable<DataPoints> datapoints);
    }
}