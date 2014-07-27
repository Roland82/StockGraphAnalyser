
namespace StockGraphAnalyser.FrontEnd.Infrastructure
{
    using System;

    public static class DateTimeExtensions
    {

        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a DateTime to its Unix timestamp value. This is the number of seconds
        /// passed since the Unix Epoch (1/1/1970 UTC)
        /// </summary>
        /// <param name="aDate">DateTime to convert</param>
        /// <returns>Number of seconds passed since 1/1/1970 UTC </returns>
        public static int ToEpoch(this DateTime aDate) {
            if (aDate == DateTime.MinValue)
            {
                return -1;
            }

 
            var span = (aDate.ToLocalTime() - UnixEpoch);
            return (int) Math.Floor(span.TotalMilliseconds);
        }
    }
}