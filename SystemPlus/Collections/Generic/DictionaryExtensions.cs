﻿using System.Diagnostics.CodeAnalysis;

namespace SystemPlus.Collections.Generic
{
    /// <summary>
    /// Extension methods for Dictionaries
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Removes a values and returns it, otherwise returns default value
        /// </summary>
        [return: MaybeNull]
        public static TValue TryTakeValue<TKey, TValue>(this IDictionary<TKey, TValue> list, TKey key) where TKey : notnull
        {
            if (list != null)
            {
                if (list.TryGetValue(key, out TValue? value))
                {
                    list.Remove(key);
                    return value;
                }
            }

            return default;
        }

        /// <summary>
        /// Adds item if key not already in collection
        /// </summary>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> list, TKey key, TValue item) where TKey : notnull
        {
            if (list == null)
                return false;

            if (list.ContainsKey(key))
                return false;

            list.Add(key, item);
            return true;
        }

        /// <summary>
        /// Adds item or sets the value if it exists already
        /// </summary>
        public static void AddOrSet<TKey, TValue>(this IDictionary<TKey, TValue> list, TKey key, TValue value) where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(list);

            if (list.ContainsKey(key))
                list[key] = value;
            else
                list.Add(key, value);
        }

        public static void Increment<TKey>(this IDictionary<TKey, int> list, TKey key) where TKey : notnull
        {
            list.Increment(key, 1);
        }

        public static void Increment<TKey>(this IDictionary<TKey, int> list, TKey key, int increment) where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(list);

            if (list.ContainsKey(key))
                list[key] += increment;
            else
                list.Add(key, increment);
        }

        public static void MergeIn<TKey, TValue>(this IDictionary<TKey, TValue> list, IDictionary<TKey, TValue> other) where TKey : notnull
        {
            if (other == null)
                return;

            foreach (KeyValuePair<TKey, TValue> kvp in other)
            {
                list.TryAdd(kvp.Key, kvp.Value);
            }
        }

        public static IDictionary<TKey, TValue> AsDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValues) where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(keyValues);

            Dictionary<TKey, TValue> dictionary = new();

            foreach (KeyValuePair<TKey, TValue> kvp in keyValues)
            {
                dictionary.TryAdd(kvp.Key, kvp.Value);
            }

            return dictionary;
        }
    }
}