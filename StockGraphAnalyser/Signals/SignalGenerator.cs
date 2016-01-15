

namespace StockGraphAnalyser.Signals
{
    using System.Collections.Generic;
    using System.Linq;
    using StockGraphAnalyser.Domain;
    using StockGraphAnalyser.Signals.TradingStrategies;

    public class SignalGenerator
    {
        private readonly IEnumerable<AbstractTradingStrategy> strategies;
        private readonly IEnumerable<DataPoints> dataPointsList;

        public SignalGenerator(IEnumerable<DataPoints> dataPointsList, IEnumerable<AbstractTradingStrategy> strategies)
        {
            this.dataPointsList = dataPointsList;
            this.strategies = strategies;
        }

        public IEnumerable<Signal> Generate() {
            AbstractTradingStrategy lastStrategyToBeUsed = null;
            SignalType? lastSignalType = null;

            foreach (var datapoints in this.dataPointsList)
            {
                if (IsNewStrategyAllowed(lastStrategyToBeUsed, lastSignalType))
                {
                    lastStrategyToBeUsed = this.strategies.FirstOrDefault(s => s.IsApplicableTo(datapoints.Date));
                }

                if (lastStrategyToBeUsed != null)
                {
                    var action = lastStrategyToBeUsed.ActionToTake(datapoints.Date, lastSignalType);
                    if (action != null)
                    {
                        lastSignalType = action.SignalType;
                        yield return action;
                    }
                }
            }
        }

        private static bool IsNewStrategyAllowed(AbstractTradingStrategy lastStrategy, SignalType? lastSignalType) {
            return lastStrategy == null || (lastSignalType.HasValue && lastSignalType.Value == SignalType.TakeProfits);
        }
    }
}
