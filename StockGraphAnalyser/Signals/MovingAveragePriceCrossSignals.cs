

namespace StockGraphAnalyser.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Processing;

    public class MovingAveragePriceCrossSignals : IGenerateSignals
    {
        private readonly IEnumerable<DataPoints> dataPoints;

        public MovingAveragePriceCrossSignals(IEnumerable<DataPoints> dataPoints) {
            this.dataPoints = dataPoints;
        }

        public IEnumerable<Signal> GenerateSignals()
        {
            Signal lastSignal = null;
            return this.dataPoints.ForEachGroup(7, group =>
                                                       {
                if (!group.ElementAt(0).MovingAverageTwoHundredDay.HasValue) return null;

                var isTrendInMovingAveragePositive = (group.ElementAt(6).MovingAverageTwentyDay > group.ElementAt(6).MovingAverageFiftyDay) && (group.ElementAt(6).MovingAverageFiftyDay > group.ElementAt(6).MovingAverageTwoHundredDay);
                

                if (lastSignal != null && lastSignal.SignalType == SignalType.Sell && group.ElementAt(6).Close > group.ElementAt(6).MovingAverageFiftyDay)
                {
                    lastSignal = Signal.Create(group.ElementAt(6).Symbol, group.ElementAt(6).Date, SignalType.TakeProfits, group.ElementAt(6).Close);
                    return lastSignal;
                }

                if (lastSignal != null && lastSignal.SignalType == SignalType.Buy && group.ElementAt(6).Close < group.ElementAt(6).MovingAverageFiftyDay)
                {
                    lastSignal = Signal.Create(group.ElementAt(6).Symbol, group.ElementAt(6).Date, SignalType.TakeProfits, group.ElementAt(6).Close);
                    return lastSignal;
                }

                if (group.ElementAt(6).Close > group.ElementAt(6).UpperBollingerBandTwoDeviation && !isTrendInMovingAveragePositive)
                {
                    if (lastSignal != null && lastSignal.SignalType == SignalType.Sell)
                    {
                        return null;
                    }

                    lastSignal = Signal.Create(group.ElementAt(6).Symbol, group.ElementAt(6).Date, SignalType.Sell, group.ElementAt(6).Close);
                    return lastSignal;
                }

                if (group.ElementAt(6).Close < group.ElementAt(6).LowerBollingerBandTwoDeviation && isTrendInMovingAveragePositive)
                {
                    if (lastSignal != null && lastSignal.SignalType == SignalType.Buy)
                    {
                        return null;
                    }


                    lastSignal = Signal.Create(group.ElementAt(6).Symbol, group.ElementAt(6).Date, SignalType.Buy, group.ElementAt(6).Close);
                    return lastSignal;
                }

                return null;
            }).Where(e => e != null);
        }

        //public IEnumerable<Signal> GenerateSignals()
        //{
        //    Signal lastSignal = null;
        //    return this.dataPoints.ForEachGroup(7, group =>
        //                                               {
        //            var isMacdPositive = group.ElementAt(6).TwelveDayVsTwentyDayEmaHistogram > 0;
        //            var isTrendInMacdPositive = group.ElementAt(6).TwelveDayVsTwentyDayEmaHistogram - group.ElementAt(3).TwelveDayVsTwentyDayEmaHistogram > 0;

        //            if (lastSignal != null && lastSignal.SignalType == SignalType.Sell && (group.ElementAt(6).Close > group.ElementAt(6).MovingAverageTwentyDay || isMacdPositive || isTrendInMacdPositive))
        //            {
        //                lastSignal = Signal.Create(group.ElementAt(6).Symbol, group.ElementAt(6).Date, SignalType.TakeProfits, group.ElementAt(6).Close);
        //                return lastSignal;
        //            }

        //            if (lastSignal != null && lastSignal.SignalType == SignalType.Buy && (group.ElementAt(6).Close < group.ElementAt(6).MovingAverageTwentyDay || !isMacdPositive || !isTrendInMacdPositive))
        //            {
        //                lastSignal = Signal.Create(group.ElementAt(6).Symbol, group.ElementAt(6).Date, SignalType.TakeProfits, group.ElementAt(6).Close);
        //                return lastSignal;
        //            }

        //            if (group.ElementAt(6).Close < group.ElementAt(6).MovingAverageTwentyDay && !isMacdPositive && !isTrendInMacdPositive)
        //            {
        //                if (lastSignal != null && lastSignal.SignalType == SignalType.Sell)
        //                {
        //                    return null; 
        //                }

        //                lastSignal = Signal.Create(group.ElementAt(6).Symbol, group.ElementAt(6).Date, SignalType.Sell, group.ElementAt(6).Close);
        //                return lastSignal;
        //            }

        //            if (group.ElementAt(6).Close > group.ElementAt(6).MovingAverageTwentyDay && isMacdPositive && isTrendInMacdPositive)
        //            {
        //                if (lastSignal != null && lastSignal.SignalType == SignalType.Buy)
        //                {
        //                    return null;
        //                }


        //                lastSignal = Signal.Create(group.ElementAt(6).Symbol, group.ElementAt(6).Date, SignalType.Buy, group.ElementAt(6).Close);
        //                return lastSignal;
        //            }

        //            return null;
        //        }).Where(e => e != null);
        //}
    }
}
