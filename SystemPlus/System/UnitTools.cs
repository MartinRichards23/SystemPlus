using System;

namespace SystemPlus
{
    /// <summary>
    /// Tools for dealing with various units
    /// </summary>
    public static class UnitTools
    {
        public const double YardsInMile = 1760;
        public const double KilometresInMile = 1.609344;
        public const double MetresInYard = 0.9144;

        public static string FormatMetres(double metres, bool metric)
        {
            return FormatMetres(metres, metric, null);
        }

        public static string FormatMetres(double metres, bool metric, IFormatProvider? provider)
        {
            // want to get it looking like this
            // 1,000 km   or  1000 mi
            if (metres < 0)
                return "N/A";

            if (metric)
            {
                if (metres < 1000)
                {
                    return metres.ToString("0", provider) + "m";
                }

                double kilometres = metres / 1000;

                if (kilometres < 10)
                    return kilometres.ToString("0.#", provider) + "km";
                return kilometres.ToString("0", provider) + "km";
            }
            else
            {
                int yards = (int)ConvertMetresToYards(metres);

                if (yards < YardsInMile)
                {
                    return yards.ToString(provider) + "yards";
                }
                double miles = ConvertYardsToMiles(yards);

                if (miles < 1)
                    return miles.ToString("0.#", provider) + "miles";

                return miles.ToString("0", provider) + "miles";
            }
        }

        public static double ConvertMilesToKilometres(double miles)
        {
            return miles * KilometresInMile;
        }

        public static double ConvertYardsToMetres(double yards)
        {
            return yards * MetresInYard;
        }

        public static double ConvertKilometresToMiles(double km)
        {
            return km / KilometresInMile;
        }

        public static double ConvertMetresToYards(double metres)
        {
            return metres / MetresInYard;
        }

        public static double ConvertYardsToMiles(double yards)
        {
            return yards / YardsInMile;
        }

        public static double ConvertMilesToYards(double miles)
        {
            return miles * YardsInMile;
        }
    }
}
