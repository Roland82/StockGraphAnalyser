
namespace StockGraphAnalyser.Processing.Candlestick
{
    using System.Collections.Generic;
    using Domain;

    public class CandlestickPatternRecogniserFactory : ICandlestickPatternRecogniserFactory
    {
        public IEnumerable<IDetectPattern> CreateAll(IEnumerable<DataPoints> datapoints) {
            yield return new EngulfingPatterRecogniser(datapoints, SentimentType.Bullish);
            yield return new EngulfingPatterRecogniser(datapoints, SentimentType.Bearish);
            yield return new KickerPatternRecogniser(datapoints, SentimentType.Bullish);
            yield return new KickerPatternRecogniser(datapoints, SentimentType.Bearish);
        } 
    }
}
