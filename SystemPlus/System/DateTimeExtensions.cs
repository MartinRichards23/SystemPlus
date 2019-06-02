using System;
using System.Globalization;

namespace SystemPlus
{
    /// <summary>
    /// Helpers functions for datetime offsets
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get the timezone that matches the given offset
        /// </summary>
        public static TimeZoneInfo GetTimeZoneByOffset(double offset)
        {
            // find best timezone
            TimeZoneInfo tzInfo = null;
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
        /// Returns a date string formatted to be file name safe. yyyy-MM-dd
        /// </summary>
        public static string ToFileDateString(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
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
        /// Returns a date string formatted to be file name safe
        /// </summary>
        public static string ToFileTimeString(this DateTime? datetime)
        {
            if (datetime == null)
                return null;

            return datetime.Value.ToString("yyyy-MM-dd_HH-mm-ss");
        }

        /// <summary>
        /// Returns a date string formatted yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string ToStringStandard(this DateTime? datetime)
        {
            if (datetime == null)
                return null;

            return datetime.Value.ToString("yyyy-MM-dd HH:mm:ss");
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
        /// Formats a timespan like '1 days 2 hours 3 minutes 4 seconds'
        /// </summary>
        public static string FormatTimeSpan(TimeSpan ts, bool includeMilliseconds = false)
        {
            string s;

            if (ts.Days > 0)
            {
                s = string.Format("{0} days {1} hours {2} minutes {3} seconds", (int)ts.TotalDays, ts.Hours, ts.Minutes, ts.Seconds);
            }
            else if (ts.Hours > 0)
            {
                s = string.Format("{0} hours {1} minutes {2} seconds", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
            }
            else if (ts.Minutes > 0)
            {
                s = string.Format("{0} minutes {1} seconds", (int)ts.TotalMinutes, ts.Seconds);
            }
            else if (ts.Seconds > 0)
            {
                s = string.Format("{0} seconds", (int)ts.TotalSeconds);
            }
            else if (ts.Milliseconds > 0 && includeMilliseconds)
            {
                s = string.Format("{0} milliseconds", (int)ts.TotalMilliseconds);
            }
            else
            {
                s = "0";
            }

            return s;
        }

        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return (long)t.TotalSeconds;
        }
    }
}