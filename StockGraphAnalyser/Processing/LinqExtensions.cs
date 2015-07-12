

namespace StockGraphAnalyser.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    public static class LinqExtensions
    {
        public static Dictionary<DateTime, decimal> SubsetBetween(this Dictionary<DateTime, decimal> dictionary, DateTime startDate, DateTime endDate)
        {
            return dictionary.Where(e => e.Key >= startDate && e.Key <= endDate).ToDictionary(e => e.Key, e => e.Value);
        }

        public static int? IndexOf<T>(this IEnumerable<T> list, Func<T, bool> predicate) {
            var i = 0;
            foreach (var k in list)
            {
                if (predicate(k))
                {
                    return i;
                }

                i++;
            }

            return null;
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

        public static void ForEachGroup<T>(this IEnumerable<T> enumerable, int groupSize, Action<IEnumerable<T>> action)
        {
            var count = enumerable.Count();

            for (var i = 0; i <= count - groupSize; i++)
            {
                action(enumerable.Skip(i).Take(groupSize));
            }
        }

        public static IEnumerable<IEnumerable<T>> GroupBatches<T>(this IEnumerable<T> enumerable, int groupSize) {
            var numIterationsRequired = (int)Math.Ceiling((decimal)enumerable.Count() / groupSize);

            for (var i = 0; i < numIterationsRequired; i++)
            {
                yield return enumerable.Skip(i * groupSize).Take(groupSize);
            }
        }

        public static IEnumerable<TResult> ForEachGroup<T, TResult>(this IEnumerable<T> enumerable, int groupSize, Func<IEnumerable<T>, TResult> action)
        {
            var count = enumerable.Count();

            for (var i = 0; i <= count - groupSize; i++)
            {
                yield return action(enumerable.Skip(i).Take(groupSize));
            }
        }
    }
}
