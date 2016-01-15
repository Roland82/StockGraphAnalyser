
namespace StockGraphAnalyser.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MathExtras
    {
        public static decimal Difference(this decimal thisNumber, decimal thatNumber)
        {
            return Math.Max(thisNumber, thatNumber) - Math.Min(thisNumber, thatNumber);
        }

        public static decimal StandardDeviation(IEnumerable<decimal> dataPoints)
        {
            var mean = dataPoints.Average();
            return (decimal)Math.Sqrt(dataPoints.Select(d => Math.Pow((double)d - (double)mean, 2)).Sum() / dataPoints.Count());
        }

        public static decimal PercentageDifferenceBetween(decimal number1, decimal number2) {
            return ((number2 - number1) / number1) * 100;
        }

        public static bool IsBetween(this decimal number, decimal thisNumber, decimal thatNumber) {
            var maxNumber = thisNumber > thatNumber ? thisNumber : thatNumber;
            var minNumber = thisNumber > thatNumber ? thatNumber : thisNumber;
            return number >= minNumber && number <= maxNumber;
        }
    }
}
