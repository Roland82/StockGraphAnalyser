

namespace StockGraphAnalyser.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StockGraphAnalyser.Domain;

    public static class LinqExtensions
    {
        public static Dictionary<DateTime, decimal> SubsetBetween(this Dictionary<DateTime, decimal> dictionary, DateTime startDate, DateTime endDate)
        {
            return dictionary.Where(e => e.Key >= startDate && e.Key <= endDate).ToDictionary(e => e.Key, e => e.Value);
        }

        public static IEnumerable<DataPoints> MapNewDataPoint(this IEnumerable<DataPoints> originalList, Dictionary<DateTime, decimal> mappingDictionary, Action<DataPoints, decimal> function)
        {
            return originalList.Select(x =>
                                           {
                                               if (mappingDictionary.Any(d => d.Key == x.Date))
                                               {
                                                   function(x, mappingDictionary.First(d => d.Key == x.Date).Value);
                                               }

                                               return x;
                                           });
        }
    }
}
