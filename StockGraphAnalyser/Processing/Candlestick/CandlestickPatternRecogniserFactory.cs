
namespace StockGraphAnalyser.Processing.Candlestick
{
    using System.Collections.Generic;
    using Domain;

    public class CandlestickPatternRecogniserFactory : ICandlestickPatternRecogniserFactory
    {
        public IEnumerable<IDetectPattern> CreateAll(IEnumerable<DataPoints> datapoints) {
            yield return new EngulfingPatterRecogniser(datapoints, EngulfingPatterRecogniser.Type.Bullish);
            yield return new EngulfingPatterRecogniser(datapoints, EngulfingPatterRecogniser.Type.Bearish);
            yield return new BullishKickerPatternRecogniser(datapoints);
        } 
    }
}
