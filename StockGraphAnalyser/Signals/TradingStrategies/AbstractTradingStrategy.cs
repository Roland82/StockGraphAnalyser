
namespace StockGraphAnalyser.Signals.TradingStrategies
{
    using System;
    using System.Collections.Generic;
    using StockGraphAnalyser.Domain;

    public abstract class AbstractTradingStrategy
    {
        protected readonly IEnumerable<DataPoints> DataPoints;

        protected AbstractTradingStrategy(IEnumerable<DataPoints> dataPoints)
        {
            this.DataPoints = dataPoints;
        }

        protected AbstractTradingStrategy(){ /* For mocking*/ }

        /// <summary>
        /// Determines whether the trading strategy could be applied to the current market conditions on the given date [the specified date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>True if the conditions are correct to use the strategy</returns>
        public abstract bool IsApplicableTo(DateTime date);

        /// <summary>Determines what action to take on the current date.</summary>
        /// <param name="date">The date.</param>
        /// <param name="lastSignalType">Last type of the signal.</param>
        /// <returns>The trading signal to act on, on the date given.</returns>
        public abstract Signal ActionToTake(DateTime date, SignalType? lastSignalType);
    }
}
