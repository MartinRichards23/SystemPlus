using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemPlus.ComponentModel
{
    public static class EnumCache
    {
        static readonly IDictionary<Type, Array> enumArrays = new Dictionary<Type, Array>();

        /// <summary>
        /// Gets array of enum values where T is the enum type
        /// </summary>
        public static T[] GetEnumValues<T>()
        {
            Type t = typeof(T);

            if (!enumArrays.ContainsKey(t))
            {
                Enum[] a = Enum.GetValues(t).Cast<Enum>().ToArray();
                Array.Sort(a, (n1, n2) => string.CompareOrdinal(n1.ToDescriptionString(), n2.ToDescriptionString()));

                enumArrays.Add(t, a.Cast<T>().ToArray());
            }

            return (T[])enumArrays[t];
        }
    }
}