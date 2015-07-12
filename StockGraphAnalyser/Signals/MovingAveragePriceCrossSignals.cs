

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
        private const decimal AcceptableRangePredictability = 80m;

        public MovingAveragePriceCrossSignals(IEnumerable<DataPoints> dataPoints) {
            this.dataPoints = dataPoints;
        }

        public IEnumerable<Signal> GenerateSignals()
        {
            Signal lastSignal = null;

            var signals = this.dataPoints.Where(d => d.Date > DateTime.Today.AddDays(-60)).ForEachGroup(20, group =>
                {
                    if (!this.IsThereEnoughData(group)) return null;
                    var currentDatapoint = group.ElementAt(19);

                    var isTrendPositive = group.All(d => d.MovingAverageTwentyDay > d.MovingAverageTwoHundredDay);
                    var isTrendNegative = group.All(d => d.MovingAverageTwentyDay < d.MovingAverageTwoHundredDay);


                    if (lastSignal != null && lastSignal.SignalType == SignalType.Sell && isTrendPositive)
                    {
                        lastSignal = Signal.Create(currentDatapoint.Symbol, currentDatapoint.Date, SignalType.TakeProfits, currentDatapoint.Close);
                        return lastSignal;
                    }

                    if (lastSignal != null && lastSignal.SignalType == SignalType.Buy && isTrendNegative)
                    {
                        lastSignal = Signal.Create(currentDatapoint.Symbol, currentDatapoint.Date, SignalType.TakeProfits, currentDatapoint.Close);
                        return lastSignal;
                    }


                    if (isTrendNegative && currentDatapoint.Close > currentDatapoint.MovingAverageTwentyDay.Value.AddPercentage(-2))
                    {
                        if (lastSignal != null && lastSignal.SignalType == SignalType.Sell) { return null; }
             
                        if (this.IsTradingRangePredictableEnough(group, d => d.MovingAverageTwentyDay.Value, ResistanceAndSupportPredictabilityCalculator.Type.Resistance))
                        {
                            lastSignal = Signal.Create(currentDatapoint.Symbol, currentDatapoint.Date, SignalType.Sell, currentDatapoint.Close);
                            return lastSignal;
                        }
                    }

                    if (isTrendPositive && currentDatapoint.Close < currentDatapoint.MovingAverageTwentyDay.Value.AddPercentage(2))
                    {
                        if (lastSignal != null && lastSignal.SignalType == SignalType.Buy) { return null; }

                        if (this.IsTradingRangePredictableEnough(group, d => d.MovingAverageTwentyDay.Value, ResistanceAndSupportPredictabilityCalculator.Type.Support))
                        {
                            lastSignal = Signal.Create(currentDatapoint.Symbol, currentDatapoint.Date, SignalType.Buy, currentDatapoint.Close);
                            return lastSignal;
                        }
                    }

                    return null;
                }).Where(e => e != null);

            return this.AddEquityTotalling(signals);
        }

        private bool IsThereEnoughData(IEnumerable<DataPoints> group) {
            return group.ElementAt(0).MovingAverageFiftyDay.HasValue &&
                   group.ElementAt(0).LowerBollingerBandTwoDeviation.HasValue && group.ElementAt(0).MovingAverageTwoHundredDay.HasValue;
        }

        private bool IsTradingRangePredictableEnough(IEnumerable<DataPoints> group, Func<DataPoints, decimal> bound, ResistanceAndSupportPredictabilityCalculator.Type predictabilityType)
        {
            var calculator = new ResistanceAndSupportPredictabilityCalculator(predictabilityType,
                                                         group.Select(d => new Tuple<DateTime, decimal, decimal>(d.Date, d.Open, d.Close)),
                                                         group.ToDictionary(d => d.Date, bound));
            var isInPredictableRange = calculator.Calculate() > 80m;
            return isInPredictableRange;
        }

        private IEnumerable<Signal> AddEquityTotalling(IEnumerable<Signal> signals) {
            if (!signals.Any())
            {
                return signals;
            }

            var equityTotaller = new SignalEquityPositionTotaller(signals, 100m);
            var totals = equityTotaller.Calculate();

            return signals.Select(s =>
                {
                    var total = totals.First(t => t.Key == s.Date);
                    s.CurrentEquity = total.Value;
                    return s;
                });
        }  
    }
}
