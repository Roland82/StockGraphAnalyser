

namespace StockGraphAnalyser.Signals
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Will generate buy/sell/take profits signals based on data given to implementors of this interface.
    /// </summary>
    interface IGenerateSignals
    {
        Dictionary<DateTime, SignalType> GenerateSignals();
    }
}
