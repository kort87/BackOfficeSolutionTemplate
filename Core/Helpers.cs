using System;
using Core.Enums;

namespace Core
{
    public static class Helpers
    {
        #region Date
        /// <summary>
        /// Give the date of Computer Start Date
        /// </summary>
        public static DateTime StartOfTheUnixWorld = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Compute a datetime base on ticks + start of time
        /// </summary>
        /// <param name="timestamp">Number of elapsed ticks</param>
        /// <returns>An UTC datetime</returns>
        public static DateTime FromUnixTimeStamp(this long timestamp)
        {
            var newTime = StartOfTheUnixWorld.AddSeconds(timestamp);
            return newTime;
        }

        /// <summary>
        /// Compute the number of ticks between start of time and datetime (in UTC)
        /// </summary>
        /// <param name="target">The current date & time</param>
        /// <returns>The number of ticks</returns>
        public static long ToUnixTimestamp(this DateTime target)
        {
            var unixTimestamp = Convert.ToInt64((target - StartOfTheUnixWorld).TotalSeconds);

            return unixTimestamp;
        }

        /// <summary>
        /// Convert an UTC DateTime in a correct JSON/JS string datetime (ISO 8601) 
        /// </summary>
        /// <param name="date">The datetime to convert</param>
        /// <returns>A JSON ISO 8601 datetime string </returns>
        public static String ToUtcJsISO(this DateTime date)
        {
            return new DateTime(date.Ticks, DateTimeKind.Utc).ToString("O");
        }

        public static DateTime StartOfDay(this DateTime theDate)
        {
            return theDate.Date;
        }

        public static DateTime EndOfDay(this DateTime theDate)
        {
            return theDate.Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime StartOfWeek(this DateTime theDate)
        {
            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek today = DateTime.Now.DayOfWeek;
            var date = theDate.AddDays(-(today - fdow)).Date;
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, theDate.Kind);
        }

        public static DateTime EndOfWeek(this DateTime theDate)
        {
            DateTime start = StartOfWeek(theDate);

            return start.AddDays(7).AddTicks(-1);
        }

        public static DateTime StartOfMonth(this DateTime theDate)
        {
            return new DateTime(theDate.Year, theDate.Month, 1, 0, 0, 0, theDate.Kind);
        }

        public static DateTime EndOfMonth(this DateTime theDate)
        {
            return theDate.StartOfMonth().AddMonths(1).AddTicks(-1);
        }

        public static DateTime StartOfYear(this  DateTime theDate)
        {
            return new DateTime(theDate.Year, 1, 1, 0, 0, 0, theDate.Kind);
        }

        public static DateTime EndOfYear(this DateTime theDate)
        {
            return new DateTime(theDate.Year, 12, 31, 23, 59, 59, 999,theDate.Kind);
        }

        public static void GetBetweenDate(this DateTime date, Periods period, out DateTime startOfPeriod, out DateTime endOfPeriod)
        {
            startOfPeriod = date;
            endOfPeriod = date;
            switch (period)
            {
                case Periods.Day:
                    startOfPeriod = startOfPeriod.StartOfDay();
                    endOfPeriod = endOfPeriod.EndOfDay();
                    break;
                case Periods.Week:
                    startOfPeriod = startOfPeriod.StartOfWeek();
                    endOfPeriod = endOfPeriod.EndOfWeek();
                    break;
                case Periods.Month:
                    startOfPeriod = startOfPeriod.StartOfMonth();
                    endOfPeriod = endOfPeriod.EndOfMonth();
                    break;
                case Periods.Year:
                    startOfPeriod = startOfPeriod.StartOfYear();
                    endOfPeriod = endOfPeriod.EndOfYear();
                    break;
            }
        }
        #endregion

        public static bool IsNull<T>(this T subject)
        {
            return ReferenceEquals(subject, null);
        }

    }
}