namespace StockGraphAnalyser.Domain.Repository.Interfaces
{
    using System.Collections.Generic;

    public interface ITradeSignalsRepository
    {
        IEnumerable<DataPoints> FindSharesAtValue();
        IEnumerable<DataPoints> FindSharesBelowLowerBollingerBand();
        IEnumerable<DataPoints> FindSharesAboveUpperBollingerBand();
    }
}