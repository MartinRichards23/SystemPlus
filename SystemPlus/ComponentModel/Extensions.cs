using System;

namespace SystemPlus.ComponentModel
{
    public static class Extensions
    {
        /// <summary>
        /// If enum implements the InfoAttribute then returns this tostring
        /// otherwise returns enum as string.
        /// </summary>
        public static string ToInfoString(this Enum value)
        {
            InfoAttribute? info = EnumTools.GetEnumInfo(value, value.GetType());

            if (info != null)
                return info.ToString();

            return value.ToString();
        }

        /// <summary>
        /// If enum implements the DescriptionAttribute then returns this tostring
        /// otherwise returns enum as string.
        /// </summary>
        public static string ToDescriptionString(this Enum value)
        {
            if (value == null)
                throw new NullReferenceException();

            string info = EnumTools.GetEnumDescription(value, value.GetType());

            if (string.IsNullOrWhiteSpace(info))
                return value.ToString();

            return info;
        }

    }
}
