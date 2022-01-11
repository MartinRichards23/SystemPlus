using System.Globalization;

namespace SystemPlus
{
    /// <summary>
    /// Extensions for TimeSpans
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Formats a timespan like '1 day 2 hours'
        /// </summary>
        public static string FormatTimeSpan(this TimeSpan ts, bool abbreviate = false)
        {
            if (ts.TotalSeconds < 1)
                return "0 secs";

            StringBuilder sb = new StringBuilder();

            if (abbreviate)
            {
                if (ts.Days > 0)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} d ", ts.Days);

                if (ts.Hours > 0 && ts.TotalDays < 5)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} h ", ts.Hours);

                if (ts.Minutes > 0 && ts.TotalHours < 24)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} m ", ts.Minutes);

                if (ts.Seconds > 0 && ts.TotalMinutes < 15)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} s ", ts.Seconds);
            }
            else
            {
                if (ts.Days > 0)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} day{1} ", ts.Days, ts.Days > 1 ? "s" : string.Empty);

                if (ts.Hours > 0 && ts.TotalDays < 5)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} hour{1} ", ts.Hours, ts.Hours > 1 ? "s" : string.Empty);

                if (ts.Minutes > 0 && ts.TotalHours < 24)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} min{1} ", ts.Minutes, ts.Minutes > 1 ? "s" : string.Empty);

                if (ts.Seconds > 0 && ts.TotalMinutes < 15)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} sec{1} ", ts.Seconds, ts.Seconds > 1 ? "s" : string.Empty);
            }

            return sb.ToString().Trim();
        }
    }
}
