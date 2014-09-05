namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;

    public interface ICalculatorFactory
    {
        ICalculationTool CreateMovingAverageCalculator(Dictionary<DateTime, decimal> dailyFigures, int period);

        ICalculationTool CreateBollingerBandCalculator(Dictionary<DateTime, decimal> dailyFigures,
                                                       Dictionary<DateTime, decimal> dailyStandardDeviation,
                                                       BollingerBandCalculator.Band band);

        ICalculationTool CreateStandardDeviationCalculator(Dictionary<DateTime, decimal> dailyFigures, int sampleSize);

        ICalculationTool CreateForceIndexCalculator(IEnumerable<Tuple<DateTime, decimal, long>> data);

        ICalculationTool CreateExponentialMovingAverageCalculator(Dictionary<DateTime, decimal> dailyFigures, int sampleSize);

        ICalculationTool CreateMomentumCalculator(Dictionary<DateTime, decimal> dailyFigures, int periodGap);

        ICalculationTool CreateDifferenceCalculator(Dictionary<DateTime, decimal> graph1, Dictionary<DateTime, decimal> graph2);
    }
}