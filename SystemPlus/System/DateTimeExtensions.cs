using System;
using System.Globalization;

namespace SystemPlus
{
    /// <summary>
    /// Extensions for DateTimes
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get the timezone that matches the given offset
        /// </summary>
        public static TimeZoneInfo? GetTimeZoneByOffset(double offset)
        {
            // find best timezone
            TimeZoneInfo? tzInfo = null;
            foreach (TimeZoneInfo info in TimeZoneInfo.GetSystemTimeZones())
            {
                if (info.BaseUtcOffset.TotalHours == offset)
                {
                    tzInfo = info;
                    break;
                }
            }

            return tzInfo;
        }

        /// <summary>
        /// Returns a date string formatted to be file name safe. yyyy-MM-dd_HH-mm-ss
        /// </summary>
        public static string ToFileTimeString(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd_HH-mm-ss");
        }

        public static DateTime ParseFileTimeString(string input)
        {
            return DateTime.ParseExact(input, "yyyy-MM-dd_HH-mm-ss", null);
        }

        /// <summary>
        /// Returns a date string formatted yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string ToStringStandard(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static DateTime ParseStringStandard(string input)
        {
            return DateTime.ParseExact(input, "yyyy-MM-dd HH:mm:ss", null);
        }

        /// <summary>
        /// Formats a datetime in user friendly way
        /// </summary>
        public static string ToStringFriendly(this DateTime utcTime, TimeZoneInfo? timeZone = null, bool addUtc = true, bool checkIsToday = true)
        {
            DateTime localTime;
            DateTime now;

            if (timeZone == null)
            {
                localTime = utcTime;
                now = DateTime.UtcNow;
            }
            else
            {
                localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
                now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            }

            string s;

            if (checkIsToday && localTime.Date == now.Date)
                s = string.Format("Today {0:HH:mm}", localTime);
            else if (localTime.Year == now.Year)
                s = localTime.ToString("d-MMM HH:mm");
            else
                s = localTime.ToString("d-MMM-yyyy HH:mm");

            if (timeZone == null && addUtc)
                s += " (UTC)";

            return s;
        }

        public static DateTime Round(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + (span.Ticks / 2) + 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTime Floor(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks / span.Ticks);
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTime Ceiling(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + span.Ticks - 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }

        /// <summary>
        /// Converts a UTC time into a local time
        /// </summary>
        public static DateTime ToLocal(this DateTime utcTime, TimeZoneInfo timeZone)
        {
            if (timeZone == null)
                return utcTime;
            else
                return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
        }

        /// <summary>
        /// Tries to convert the string into a DateTimeOffset, returns null if parse fails
        /// </summary>
        public static DateTime? TryParse(string input)
        {
            if (DateTime.TryParse(input, out DateTime date))
                return date;

            return null;
        }

        /// <summary>
        /// Tries to convert the string into a DateTimeOffset, returns null if parse fails
        /// </summary>
        public static DateTime? TryParseExact(string input, string format)
        {
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                return date;

            return null;
        }

        /// <summary>
        /// Tries to convert the string into a DateTimeOffset, returns null if parse fails
        /// </summary>
        public static DateTime? TryParseExact(string input, string[] formats)
        {
            if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                return date;

            return null;
        }

        /// <summary>
        /// Returns the earlier datetime
        /// </summary>
        public static DateTime MinTime(DateTime val1, DateTime val2)
        {
            if (val1 < val2)
                return val1;
            return val2;
        }

        /// <summary>
        /// Returns the later datetime
        /// </summary>
        public static DateTime MaxTime(DateTime val1, DateTime val2)
        {
            if (val1 > val2)
                return val1;
            return val2;
        }

        /// <summary>
        /// Gets the Unix time
        /// </summary>
        public static long ToUnixTime(this DateTime dateTime)
        {
            TimeSpan t = dateTime - new DateTime(1970, 1, 1);
            return (long)t.TotalSeconds;
        }
    }
}