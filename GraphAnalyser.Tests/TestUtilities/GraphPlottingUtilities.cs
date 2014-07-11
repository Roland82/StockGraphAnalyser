namespace GraphAnalyser.Tests.TestUtilities
{
    using System;
    using System.Collections.Generic;

    public static class GraphPlottingUtilities
    {
        public static Dictionary<DateTime, decimal> CreateGraph(DateTime startDate, decimal[] values, bool skipWeekends = false)
        {
            var returnDictionary = new Dictionary<DateTime, decimal>();

            for (int i = 0; i < values.Length; i++)
            {               
                returnDictionary.Add(startDate, values[i]);
                startDate = startDate.AddDaysSkippingWeekends(1);
            }

            return returnDictionary;
        }

        private static DateTime AddDaysSkippingWeekends(this DateTime d, int numberOfDays)
        {
            if (d.AddDays(numberOfDays).DayOfWeek == DayOfWeek.Saturday || d.AddDays(numberOfDays).DayOfWeek == DayOfWeek.Sunday)
            {
                return d.AddDaysSkippingWeekends(numberOfDays + 1);
            }

            return d.AddDays(numberOfDays);
        }
    }
}