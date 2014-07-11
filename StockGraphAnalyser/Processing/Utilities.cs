

namespace StockGraphAnalyser.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Utilities
    {
        public static Tuple<DateTime, DateTime> CalculateSharedDateRange(IEnumerable<DateTime> dateList1, IEnumerable<DateTime> dateList2)
        {
            var intersection = dateList1.Intersect(dateList2);
            return intersection.Any() ? Tuple.Create(intersection.Min(), intersection.Max()) : null;
        }
    }
}
