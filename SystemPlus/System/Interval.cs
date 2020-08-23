using System;

namespace SystemPlus
{
    /// <summary>
    /// Common intervals
    /// </summary>
    public enum Interval
    {
        Mins1 = 1,
        Mins2 = 2,
        Mins5 = 3,
        Mins15 = 4,
        Mins30 = 5,
        Hour1 = 6,
        Hour2 = 7,
        Hour4 = 8,
        Hour8 = 9,
        Hour12 = 10,
        Day1 = 11,
        Day2 = 12,
        Week1 = 13,
        Week2 = 14,
    }

    public static class IntervalExtensions
    {
        public static string ToTimeString(this Interval interval)
        {
            return interval switch
            {
                Interval.Mins1 => "1 minute",
                Interval.Mins2 => "2 minutes",
                Interval.Mins5 => "5 minutes",
                Interval.Mins15 => "15 minutes",
                Interval.Mins30 => "30 minutes",
                Interval.Hour1 => "1 Hour",
                Interval.Hour2 => "2 hours",
                Interval.Hour4 => "4 hours",
                Interval.Hour8 => "8 hours",
                Interval.Hour12 => "12 hours",
                Interval.Day1 => "Daily",
                Interval.Day2 => "2 days",
                Interval.Week1 => "Weekly",
                Interval.Week2 => "2 Weeks",
                _ => "",
            };
        }

        public static TimeSpan ToTimespan(this Interval interval)
        {
            return interval switch
            {
                Interval.Mins1 => TimeSpan.FromMinutes(1),
                Interval.Mins2 => TimeSpan.FromMinutes(2),
                Interval.Mins5 => TimeSpan.FromMinutes(5),
                Interval.Mins15 => TimeSpan.FromMinutes(15),
                Interval.Mins30 => TimeSpan.FromMinutes(30),
                Interval.Hour1 => TimeSpan.FromHours(1),
                Interval.Hour2 => TimeSpan.FromHours(2),
                Interval.Hour4 => TimeSpan.FromHours(4),
                Interval.Hour8 => TimeSpan.FromHours(8),
                Interval.Hour12 => TimeSpan.FromHours(12),
                Interval.Day1 => TimeSpan.FromDays(1),
                Interval.Day2 => TimeSpan.FromDays(2),
                Interval.Week1 => TimeSpan.FromDays(7),
                Interval.Week2 => TimeSpan.FromDays(14),
                _ => throw new Exception("Unknown value"),
            };
        }
    }
}
