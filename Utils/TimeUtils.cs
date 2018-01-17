using System;

namespace Helium24
{
    class TimeUtils
    {
        private const string IndexDateTimeFormatString = "yyyy-MM-dd-HH-mm";

        /// <summary>
        /// Returns the date/time to the nearest 10-minutes
        /// </summary>
        /// <returns>The time to nearest 10-minutes</returns>
        public static string GetDateTimeToNearestTenMinutes()
        {
            DateTime time = DateTime.UtcNow;
            time = time.AddMinutes(-(time.Minute % 10));
            return time.ToString(TimeUtils.IndexDateTimeFormatString);
        }

        /// <summary>
        /// Gets the last day (24 hours) in 10-minute intervals, sorted from oldest to newest
        /// </summary>
        /// <returns>144 entries of the last day in 10-minute intervals</returns>
        public static string[] GetLastDayInTenMinutes()
        {
            string[] times = new string[144];
            DateTime time = DateTime.UtcNow;
            time = time.AddMinutes(-(time.Minute % 10));

            for (int i = 0; i < times.Length; i++)
            {
                times[i] = time.ToString(TimeUtils.IndexDateTimeFormatString);
                time = time.AddMinutes(-10);
            }

            return times;
        }
    }
}
