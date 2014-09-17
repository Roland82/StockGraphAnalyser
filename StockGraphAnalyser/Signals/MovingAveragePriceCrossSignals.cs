

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

        public MovingAveragePriceCrossSignals(IEnumerable<DataPoints> dataPoints) {
            this.dataPoints = dataPoints;
        }

        public IEnumerable<Signal> GenerateSignals()
        {
            Signal lastSignal = null;

            var signals = this.dataPoints.ForEachGroup(20, group =>
                {
                    if (!this.IsThereEnoughData(group)) return null;
                    var currentDatapoint = group.ElementAt(19);
                    var previousDatapoint = group.ElementAt(18);

                    var isTrendInMovingAveragePositive = MathExtras.PercentageDifferenceBetween(previousDatapoint.MovingAverageFiftyDay.Value, currentDatapoint.MovingAverageFiftyDay.Value) > 0m;
                    var isTrendInMovingAverageNegative = MathExtras.PercentageDifferenceBetween(previousDatapoint.MovingAverageFiftyDay.Value, currentDatapoint.MovingAverageFiftyDay.Value) < -0m;
                   

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
                        if (isTrendInMovingAverageNegative && currentDatapoint.Close > currentDatapoint.UpperBollingerBandTwoDeviation)
                        {
                            if (lastSignal != null && lastSignal.SignalType == SignalType.Sell) { return null; }
                            lastSignal = Signal.Create(currentDatapoint.Symbol, currentDatapoint.Date, SignalType.Sell, currentDatapoint.Close);
                            return lastSignal;
                        }

                        if (isTrendInMovingAveragePositive && currentDatapoint.Close < currentDatapoint.LowerBollingerBandTwoDeviation)
                        {
                            if (lastSignal != null && lastSignal.SignalType == SignalType.Buy) { return null; }
                            lastSignal = Signal.Create(currentDatapoint.Symbol, currentDatapoint.Date, SignalType.Buy, currentDatapoint.Close);
                            return lastSignal;
                        }

                        return null;
                    }

                    return null;
                }).Where(e => e != null);

            return this.AddEquityTotalling(signals);
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

        private IEnumerable<Signal> AddEquityTotalling(IEnumerable<Signal> signals) {
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
