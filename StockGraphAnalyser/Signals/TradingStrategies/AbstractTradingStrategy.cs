
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

        /// <summary>
        /// Determines whether the trading strategy could be applied to the current market conditions on the given date [the specified date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>True if the conditions are correct to use the strategy</returns>
        public abstract bool IsApplicableTo(DateTime date);

        /// <summary>Determines if we should take a position on the current date.</summary>
        /// <param name="date">The date.</param>
        /// <returns>True if a position should be taken.</returns>
        public abstract bool SatisfiesCriteria(DateTime date);
        
        /// <summary>Determines if we should take profits/losses from a position on the current date.</summary>
        /// <param name="date">The date.</param>
        /// <returns>True if profits should be taken.</returns>
        public abstract bool SatisfiesTakeProfitsCriteria(DateTime date);
    }
}
