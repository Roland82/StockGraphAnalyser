

namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Collections.Generic;
    using Types;

    public class FractalSearcher
    {
        private int ocurringOverPeriodOfDays;
        private Dictionary<DateTime, Quote> priceAction;

        public FractalSearcher(int ocurringOverPeriodOfDays, Dictionary<DateTime, Quote> priceAction) {
            AssertArguments.EqualTo(ocurringOverPeriodOfDays % 2, 1);

            this.ocurringOverPeriodOfDays = ocurringOverPeriodOfDays;
            this.priceAction = priceAction;
        }
    }
}
