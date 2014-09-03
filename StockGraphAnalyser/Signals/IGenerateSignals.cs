

namespace StockGraphAnalyser.Signals
{
    using System.Collections.Generic;
    using StockGraphAnalyser.Domain;

    /// <summary>
    /// Will generate buy/sell/take profits signals based on data given to implementors of this interface.
    /// </summary>
    interface IGenerateSignals
    {
        IEnumerable<Signal> GenerateSignals();
    }
}
