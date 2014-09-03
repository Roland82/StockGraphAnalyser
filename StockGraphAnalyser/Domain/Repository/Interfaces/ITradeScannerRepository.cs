namespace StockGraphAnalyser.Domain.Repository.Interfaces
{
    using System.Collections.Generic;

    public interface ITradeScannerRepository
    {
        IEnumerable<DataPoints> FindSharesAtValue();
        IEnumerable<DataPoints> FindSharesBelowLowerBollingerBand();
        IEnumerable<DataPoints> FindSharesAboveUpperBollingerBand();
    }
}