

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;

    public class CalculatorFactory : ICalculatorFactory
    {
        public ICalculationTool CreateMovingAverageCalculator(IReadOnlyDictionary<DateTime, decimal> dailyFigures, int period) 
        {
            return new DailyMovingAverageCalculator(dailyFigures, period);
        }

        public ICalculationTool CreateBollingerBandCalculator(Dictionary<DateTime, decimal> dailyFigures, Dictionary<DateTime, decimal> dailyStandardDeviation, BollingerBandCalculator.Band band) 
        {
            return new BollingerBandCalculator(dailyFigures, dailyStandardDeviation, band);
        }

        public ICalculationTool CreateStandardDeviationCalculator(IReadOnlyDictionary<DateTime, decimal> dailyFigures, int sampleSize) 
        {
            return new StandardDeviationCalculator(dailyFigures, sampleSize);
        }

        public ICalculationTool CreateForceIndexCalculator(IEnumerable<Tuple<DateTime, decimal,long>> data)
        {
            return new ForceIndexCalculator(data);
        }

        public ICalculationTool CreateExponentialMovingAverageCalculator(IReadOnlyDictionary<DateTime, decimal> dailyFigures, int sampleSize)
        {
            return new ExponentialMovingAverageCalculator(dailyFigures, sampleSize);
        }

        public ICalculationTool CreateMomentumCalculator(Dictionary<DateTime, decimal> dailyFigures, int periodGap) 
        {
            return new MomentumCalculator(dailyFigures, periodGap);
        }

        public ICalculationTool CreateDifferenceCalculator(Dictionary<DateTime, decimal> graph1, Dictionary<DateTime, decimal> graph2)
        {
            return new DifferenceCalculator(graph1, graph2);
        }
    }
}
