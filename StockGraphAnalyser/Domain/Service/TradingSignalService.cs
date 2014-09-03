
namespace StockGraphAnalyser.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Repository.Interfaces;

    public class TradingSignalService : ITradingSignalService
    {
        private readonly ITradeScannerRepository tradeScannerRepository;

        public TradingSignalService(ITradeScannerRepository tradeScannerRepository) {
            this.tradeScannerRepository = tradeScannerRepository;
        }

        public IEnumerable<DataPoints> GetDatapointsForTradeSignal(SignalType signalType) {
            var queries = new Dictionary<SignalType, Func<IEnumerable<DataPoints>>>
                {
                    { SignalType.InValueZone, this.tradeScannerRepository.FindSharesAtValue },
                    { SignalType.AboveUpperBollingerBand, this.tradeScannerRepository.FindSharesAboveUpperBollingerBand },
                    { SignalType.BelowLowerBollingerBand, this.tradeScannerRepository.FindSharesBelowLowerBollingerBand }
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
