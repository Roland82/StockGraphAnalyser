

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Responsible for looking at a list of prices and working out if the price is going higher/lower over
    /// a set period looking back from todays date.
    /// </summary>
    public class GeneralMovementCalculator : ICalculationTool
    {
        private readonly Dictionary<DateTime, decimal> priceAction;
        private readonly int dayThreshold;

        public GeneralMovementCalculator(Dictionary<DateTime, decimal> priceAction, int dayThreshold) {
            this.priceAction = priceAction;
            this.dayThreshold = dayThreshold;
        }

        /// <summary>
        /// Calculates the general price movement over a threshold of days across all the prices given.
        /// </summary>
        /// <returns></returns>
        public Task<Dictionary<DateTime, decimal>> Calculate()
        {
            return Task.Run(() =>
                                {
                                    var returnCalculations = new Dictionary<DateTime, decimal>();
                                    var prices = this.priceAction.OrderBy(d => d.Key);

                                    // Take 2 prices of the given day distance and using pythagorus work out the length of hypotinous.
                                    // Then compare to what the length would be if the price action was flat thus resulting in a positive/negative number. 
                                    for (var i = 0; i <= this.priceAction.Count - this.dayThreshold; i++)
                                    {
                                        var startPrice = prices.Skip(i).First();
                                        var endPrice = prices.Skip(i + this.dayThreshold - 1).First();
                                        var actualDayDistance = endPrice.Key.Subtract(startPrice.Key).TotalDays;
                                        var flatPriceLength = (decimal) Math.Sqrt(Math.Pow(actualDayDistance, 2));
                                        var hypotinousLength = (decimal) Math.Sqrt(Math.Pow((double) startPrice.Value.Difference(endPrice.Value), 2) + Math.Pow(actualDayDistance, 2));
                                        var difference = startPrice.Value < endPrice.Value ? hypotinousLength.Difference(flatPriceLength) : -hypotinousLength.Difference(flatPriceLength);
                                        returnCalculations.Add(prices.ElementAt(i + this.dayThreshold - 1).Key, difference);
                                    }

                                    return returnCalculations;
                                });
        }
    }
}
