
namespace StockGraphAnalyser.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MathExtras
    {
        public static decimal Difference(this decimal thisNumber, decimal thatNumber)
        {
            return Math.Max(thisNumber, thatNumber) - System.Math.Min(thisNumber, thatNumber);
        }

        public static decimal StandardDeviation(IEnumerable<decimal> dataPoints)
        {
            var mean = dataPoints.Average();
            return (decimal)Math.Sqrt(dataPoints.Select(d => Math.Pow((double)d - (double)mean, 2)).Sum() / dataPoints.Count());
        }

        public static decimal PercentageDifferenceBetween(decimal number, decimal anotherNumber)
        {
            return ((number - anotherNumber)/((number + anotherNumber)/2))*100;
        }
    }
}
