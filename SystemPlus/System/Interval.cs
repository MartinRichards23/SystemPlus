using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPlus
{
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
            switch (interval)
            {
                case Interval.Mins1:
                    return "1 minute";
                case Interval.Mins2:
                    return "2 minutes";
                case Interval.Mins5:
                    return "5 minutes";
                case Interval.Mins15:
                    return "15 minutes";
                case Interval.Mins30:
                    return "30 minutes";

                case Interval.Hour1:
                    return "1 Hour";
                case Interval.Hour2:
                    return "2 hours";
                case Interval.Hour4:
                    return "4 hours";
                case Interval.Hour8:
                    return "8 hours";
                case Interval.Hour12:
                    return "12 hours";

                case Interval.Day1:
                    return "Daily";
                case Interval.Day2:
                    return "2 days";

                case Interval.Week1:
                    return "Weekly";
                case Interval.Week2:
                    return "2 Weeks";

                default:
                    return "";
            }
        }

        public static TimeSpan ToTimespan(this Interval interval)
        {
            switch (interval)
            {
                case Interval.Mins1:
                    return TimeSpan.FromMinutes(1);
                case Interval.Mins2:
                    return TimeSpan.FromMinutes(2);
                case Interval.Mins5:
                    return TimeSpan.FromMinutes(5);
                case Interval.Mins15:
                    return TimeSpan.FromMinutes(15);
                case Interval.Mins30:
                    return TimeSpan.FromMinutes(30);

                case Interval.Hour1:
                    return TimeSpan.FromHours(1);
                case Interval.Hour2:
                    return TimeSpan.FromHours(2);
                case Interval.Hour4:
                    return TimeSpan.FromHours(4);
                case Interval.Hour8:
                    return TimeSpan.FromHours(8);
                case Interval.Hour12:
                    return TimeSpan.FromHours(12);

                case Interval.Day1:
                    return TimeSpan.FromDays(1);
                case Interval.Day2:
                    return TimeSpan.FromDays(2);

                case Interval.Week1:
                    return TimeSpan.FromDays(7);
                case Interval.Week2:
                    return TimeSpan.FromDays(14);
                default:
                    throw new Exception("Unknown value");
            }
        }
    }
}
