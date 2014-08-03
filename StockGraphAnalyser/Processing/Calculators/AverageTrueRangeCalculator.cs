
namespace StockGraphAnalyser.Processing.Calculators
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Types;

    /// <summary>
    /// Wilder started with a concept called True Range (TR), which is defined as the greatest of the following:
    /// Method 1: Current High less the current Low
    /// Method 2: Current High less the previous Close (absolute value)
    /// Method 3: Current Low less the previous Close (absolute value)
    /// </summary>
    public class AverageTrueRangeCalculator : ICalculationTool
    {
        private readonly IEnumerable<Quote> quotes;
        private readonly int sampleSize;

        public AverageTrueRangeCalculator(IEnumerable<Quote> quotes, int sampleSize)
        {
            AssertArguments.GreaterThanOrEqualTo(quotes.Count(), sampleSize);
            this.quotes = quotes;
            this.sampleSize = sampleSize;
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync()
        {
            return Task.Run(() =>
                                {
                                    var averageTrueRanges = new Dictionary<DateTime, decimal>();
                                    var ranges = this.CalculateRanges();
                                    var limit = this.quotes.Count() - this.sampleSize;

                                    for (var x = 0; x <= limit; x++)
                                    {
                                        averageTrueRanges.Add(
                                            ranges.Skip(x).First().Key,
                                            ranges.Skip(x).Take(this.sampleSize).Average(r => r.Value)
                                            );
                                    }

                                    return averageTrueRanges;
                                });
        }

        public Task<Dictionary<DateTime, decimal>> CalculateAsync(DateTime fromDate) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculate all the true ranges for each day.
        /// </summary>
        /// <returns>A list of dates and their true range compared to the last days price action.</returns>
        private Dictionary<DateTime, decimal> CalculateRanges() {
            Quote lastQuote = null;
            var ranges = new Dictionary<DateTime, decimal>();

            foreach (var quote in this.quotes)
            {
                if (lastQuote == null)
                {
                    ranges.Add(quote.Date, quote.High - quote.Low);
                    lastQuote = quote;
                    continue;
                }

                var possibleRanges = new[]
                    {
                        Math.Abs(quote.High - lastQuote.Close),
                        Math.Abs(quote.Low - lastQuote.Close),
                        quote.High.Difference(quote.Low),
                    };

                ranges.Add(quote.Date, possibleRanges.Max());
                lastQuote = quote;
            }

            return ranges;
        }
    }
}
