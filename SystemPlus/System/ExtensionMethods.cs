using System;
using System.Reflection;
using System.Text;

namespace SystemPlus
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Lists all properties and their values
        /// </summary>
        public static string ToPropertiesString(this object obj, string seperator, bool ignoreReadonly)
        {
            StringBuilder sb = new StringBuilder();

            Type t = obj.GetType();
            foreach (PropertyInfo info in t.GetProperties())
            {
                if (ignoreReadonly && !info.CanWrite)
                    continue;

                object val = info.GetValue(obj, null);
                string textVal;

                if (val == null)
                    textVal = "null";
                else
                    textVal = val.ToString();

                sb.Append(info.Name + ": " + textVal).Append(seperator);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Determins if a type can be assigned a null value
        /// </summary>
        public static bool IsNullable(this Type type)
        {
            return !type.IsValueType || (Nullable.GetUnderlyingType(type) != null);
        }

        /// <summary>
        /// Determines if a comparable type is between 2 others.
        /// </summary>
        public static bool Between<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            return actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) < 0;
        }

        /// <summary>
        /// Clamps a comparible value
        /// </summary>
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }

        /// <summary>
        /// Returns item as array with a single item in it
        /// </summary>
        public static T[] AsArray<T>(this T item)
        {
            return new[] { item };
        }

        public static string FormatVersion(this Version v)
        {
            if (v.Revision > 0)
                return string.Format("{0}.{1}.{2}.{3}", v.Major, v.Minor, v.Build, v.Revision);

            if (v.Build > 0)
                return string.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build);

            return string.Format("{0}.{1}", v.Major, v.Minor);
        }
    }

    public interface ICloneable<out T> : ICloneable where T : ICloneable<T>
    {
        new T Clone();
    }
}