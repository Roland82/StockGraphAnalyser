namespace StockGraphAnalyser.Domain.Service.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface ITradeSignalManagementService
    {
        void GenerateNewSignals();
        void GenerateNewSignals(string company);
        IEnumerable<Signal> GetAll(DateTime fromDate);
        IEnumerable<Signal> GetAll(string symbol);
    }
}