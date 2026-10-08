using System.Globalization;

namespace SystemPlus
{
    /// <summary>
    /// Helper tools for parsing data
    /// </summary>
    public static class ParseTools
    {
        /// <summary>
        /// Parses an enum value, using the default if it fails
        /// </summary>
        public static TEnum Enum<TEnum>(string? value, TEnum defaultVal) where TEnum : struct
        {
            if (System.Enum.TryParse(value, true, out TEnum val))
                return val;

            return defaultVal;
        }

        public static int? Int(string? s)
        {
            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int val))
                return val;

            return null;
        }

        public static double? Double(string? s)
        {
            if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double val))
                return val;

            return null;
        }

        public static float? Float(string? s)
        {
            if (float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out float val))
                return val;

            return null;
        }
    }
}
