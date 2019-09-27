using System;

namespace SystemPlus
{
    /// <summary>
    /// Enum tools and extensions
    /// </summary>
    public static class EnumTools
    {
        /// <summary>
        /// Parses an enum value, using the default if it fails
        /// </summary>
        public static TEnum Parse<TEnum>(string value, TEnum defaultVal) where TEnum : struct
        {
            if (Enum.TryParse(value, true, out TEnum val))
                return val;

            return defaultVal;
        }
    }
}
