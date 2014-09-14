

namespace StockGraphAnalyser.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Utilities
    {
        public static Tuple<DateTime, DateTime> CalculateSharedDateRange(IEnumerable<DateTime> dateList1, IEnumerable<DateTime> dateList2)
        {
            var intersection = dateList1.Intersect(dateList2);
            return intersection.Any() ? Tuple.Create(intersection.Min(), intersection.Max()) : null;
        }

        public static string CommaSeparatedValues<T, TMappedValue>(T[] input, Func<T, TMappedValue> mapValue) {
            var stringBuilder = new StringBuilder();
            var comma = "";
            foreach (var value in input)
            {
                stringBuilder.Append(comma + mapValue(value));
                comma = ",";
            }

            return stringBuilder.ToString();
        }
    }
}
