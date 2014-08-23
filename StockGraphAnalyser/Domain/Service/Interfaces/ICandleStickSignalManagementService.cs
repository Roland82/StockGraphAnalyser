namespace StockGraphAnalyser.Domain.Service.Interfaces
{
    using System;

    public interface ICandleStickSignalManagementService
    {
        void GenerateLatestSignals(string symbol, DateTime fromDate);
    }
}