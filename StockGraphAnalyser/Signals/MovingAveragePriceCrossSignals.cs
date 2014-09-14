

namespace StockGraphAnalyser.Signals
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Domain;
    using Processing;
    using Processing.Calculators;

    public class MovingAveragePriceCrossSignals : IGenerateSignals
    {
        private readonly IEnumerable<DataPoints> dataPoints;
        private readonly IEnumerable<CandleStickSignal> candlestickPatterns;

        public MovingAveragePriceCrossSignals(IEnumerable<DataPoints> dataPoints, IEnumerable<CandleStickSignal> candlestickPatterns)
        {
            this.dataPoints = dataPoints;
            this.candlestickPatterns = candlestickPatterns;
        }

        public IEnumerable<Signal> GenerateSignals()
        {
            Signal lastSignal = null;

            return this.dataPoints.ForEachGroup(30, group =>
                {
                    if (!this.IsThereEnoughData(group)) return null;


                    var isTrendInMovingAveragePositive = (group.ElementAt(29).MovingAverageFiftyDay > group.ElementAt(25).MovingAverageFiftyDay);
                    var isTrendInMovingAverageNegative = (group.ElementAt(29).MovingAverageFiftyDay < group.ElementAt(25).MovingAverageFiftyDay);
                    var currentDatapoint = group.ElementAt(29);

                    if (lastSignal != null && lastSignal.SignalType == SignalType.Sell && (currentDatapoint.Close < currentDatapoint.LowerBollingerBandOneDeviation || isTrendInMovingAveragePositive))
                    {
                        lastSignal = Signal.Create(currentDatapoint.Symbol, currentDatapoint.Date, SignalType.TakeProfits, currentDatapoint.Close);
                        return lastSignal;
                    }

                    if (lastSignal != null && lastSignal.SignalType == SignalType.Buy
                       && (currentDatapoint.Close > currentDatapoint.UpperBollingerBandOneDeviation || isTrendInMovingAverageNegative))
                    {
                        lastSignal = Signal.Create(currentDatapoint.Symbol, currentDatapoint.Date, SignalType.TakeProfits, currentDatapoint.Close);
                        return lastSignal;
                    }

                    if (this.IsTradingRangePredictableEnough(group))
                    {
                        if (isTrendInMovingAverageNegative &&
                            group.Skip(25).Take(5).Any(d => d.Close > d.UpperBollingerBandTwoDeviation))
                        {
                            if (lastSignal != null && lastSignal.SignalType == SignalType.Sell)
                            {
                                return null;
                            }

                            lastSignal = Signal.Create(currentDatapoint.Symbol, currentDatapoint.Date,
                                                       SignalType.Sell, currentDatapoint.Close);
                            return lastSignal;

                        }

                        if (isTrendInMovingAveragePositive &&
                            group.Skip(25).Take(5).Any(d => d.Close < d.LowerBollingerBandTwoDeviation))
                        {
                            if (lastSignal != null && lastSignal.SignalType == SignalType.Buy)
                            {
                                return null;
                            }


                            lastSignal = Signal.Create(currentDatapoint.Symbol, currentDatapoint.Date,
                                                       SignalType.Buy, currentDatapoint.Close);
                            return lastSignal;


                        }

                        return null;
                    }

                    return null;
                }).Where(e => e != null);
        }

        private bool IsThereEnoughData(IEnumerable<DataPoints> group) {
            return group.ElementAt(0).MovingAverageFiftyDay.HasValue &&
                   group.ElementAt(0).LowerBollingerBandTwoDeviation.HasValue;
        }

        private bool IsTradingRangePredictableEnough(IEnumerable<DataPoints> group)
        {
            var rangePredictabilityCalculator = new RangePredictabilityCalculator(
                                                         group.Select(d => new Tuple<DateTime, decimal, decimal>(d.Date, d.Open, d.Close)),
                                                         group.ToDictionary(d => d.Date,d => d.LowerBollingerBandTwoDeviation.Value),
                                                         group.ToDictionary(d => d.Date, d => d.UpperBollingerBandTwoDeviation.Value));
            var isInPredictableRange = rangePredictabilityCalculator.Calculate() > 80m;
            return isInPredictableRange;
        }

        //public IEnumerable<Signal> GenerateSignals()
        //{
        //    Signal lastSignal = null;
        //    return this.dataPoints.ForEachGroup(7, group =>
        //                                               {
        //            var isMacdPositive = group.ElementAt(89).MacdTwentyTwoOverTwelveDay > 0;
        //            var isTrendInMacdPositive = group.ElementAt(89).MacdTwentyTwoOverTwelveDay - group.ElementAt(3).MacdTwentyTwoOverTwelveDay > 0;

        //            if (lastSignal != null && lastSignal.SignalType == SignalType.Sell && (group.ElementAt(89).Close > group.ElementAt(89).MovingAverageTwentyDay || isMacdPositive || isTrendInMacdPositive))
        //            {
        //                lastSignal = Signal.Create(group.ElementAt(89).Symbol, group.ElementAt(89).Date, SignalType.TakeProfits, group.ElementAt(89).Close);
        //                return lastSignal;
        //            }

        //            if (lastSignal != null && lastSignal.SignalType == SignalType.Buy && (group.ElementAt(89).Close < group.ElementAt(89).MovingAverageTwentyDay || !isMacdPositive || !isTrendInMacdPositive))
        //            {
        //                lastSignal = Signal.Create(group.ElementAt(89).Symbol, group.ElementAt(89).Date, SignalType.TakeProfits, group.ElementAt(89).Close);
        //                return lastSignal;
        //            }

        //            if (group.ElementAt(89).Close < group.ElementAt(89).MovingAverageTwentyDay && !isMacdPositive && !isTrendInMacdPositive)
        //            {
        //                if (lastSignal != null && lastSignal.SignalType == SignalType.Sell)
        //                {
        //                    return null; 
        //                }

        //                lastSignal = Signal.Create(group.ElementAt(89).Symbol, group.ElementAt(89).Date, SignalType.Sell, group.ElementAt(89).Close);
        //                return lastSignal;
        //            }

        //            if (group.ElementAt(89).Close > group.ElementAt(89).MovingAverageTwentyDay && isMacdPositive && isTrendInMacdPositive)
        //            {
        //                if (lastSignal != null && lastSignal.SignalType == SignalType.Buy)
        //                {
        //                    return null;
        //                }


        //                lastSignal = Signal.Create(group.ElementAt(89).Symbol, group.ElementAt(89).Date, SignalType.Buy, group.ElementAt(89).Close);
        //                return lastSignal;
        //            }

        //            return null;
        //        }).Where(e => e != null);
        //}
    }
}
