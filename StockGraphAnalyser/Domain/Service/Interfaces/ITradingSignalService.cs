namespace StockGraphAnalyser.Domain.Service.Interfaces
{
    using System.Collections.Generic;

    public interface ITradingSignalService
    {
        IEnumerable<DataPoints> GetDatapointsForTradeSignal(TradingSignalService.SignalType signalType);
    }
}