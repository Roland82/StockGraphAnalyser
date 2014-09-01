namespace StockGraphAnalyser.Domain.Service.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface ICandleStickSignalManagementService
    {
        void GenerateLatestSignals(string symbol, DateTime fromDate);
        void GenerateLatestSignals(DateTime fromDate);
        IEnumerable<CandleStickSignal> GetAllSignalsForCompany(string symbol);
    }
}