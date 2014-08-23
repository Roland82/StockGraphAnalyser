
namespace StockGraphAnalyser.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Repository.Interfaces;

    public class TradingSignalService : ITradingSignalService
    {
        private readonly ITradeSignalsRepository tradeSignalsRepository;

        public TradingSignalService(ITradeSignalsRepository tradeSignalsRepository) {
            this.tradeSignalsRepository = tradeSignalsRepository;
        }

        public IEnumerable<DataPoints> GetDatapointsForTradeSignal(SignalType signalType) {
            var queries = new Dictionary<SignalType, Func<IEnumerable<DataPoints>>>
                {
                    { SignalType.InValueZone, tradeSignalsRepository.FindSharesAtValue },
                    { SignalType.AboveUpperBollingerBand, tradeSignalsRepository.FindSharesAboveUpperBollingerBand },
                    { SignalType.BelowLowerBollingerBand, tradeSignalsRepository.FindSharesBelowLowerBollingerBand }
                };

            return queries[signalType].Invoke();
        }

        public enum SignalType
        {
            InValueZone,
            AboveUpperBollingerBand,
            BelowLowerBollingerBand
        }
    }
}
