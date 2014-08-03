namespace GraphAnalyser.Tests.TestUtilities
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public static class GraphPlottingUtilities
    {
        public static Dictionary<DateTime, decimal> CreateGraph(DateTime startDate, decimal[] values, DateTime[] skipDays = null)
        {
            var returnDictionary = new Dictionary<DateTime, decimal>();

            foreach (var value in values)
            {
                returnDictionary.Add(startDate, value);
                startDate = startDate.AddDaysSkippingWeekends(1, skipDays);
            }

            return returnDictionary;
        }

        private static DateTime AddDaysSkippingWeekends(this DateTime d, int numberOfDays, IEnumerable<DateTime> skipDays) {
            var nextDay = d.AddDays(numberOfDays);

            if (nextDay.DayOfWeek == DayOfWeek.Saturday || nextDay.DayOfWeek == DayOfWeek.Sunday || (skipDays != null && skipDays.Any(date => date == nextDay)))
            {
                return d.AddDaysSkippingWeekends(numberOfDays + 1, skipDays);
            }

            return d.AddDays(numberOfDays);
        }
    }
}