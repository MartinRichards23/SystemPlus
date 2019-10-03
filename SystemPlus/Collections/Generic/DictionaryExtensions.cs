using System.Collections.Generic;

namespace SystemPlus.Collections.Generic
{
    /// <summary>
    /// Extension methods for Dictionaries
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets a value, returns default if no key
        /// </summary>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> list, TKey key, TValue defaultValue = default(TValue))
        {
            if (list.ContainsKey(key))
            {
                TValue value = list[key];
                return value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Removes a values and returns it, otherwise returns default value
        /// </summary>
        public static TValue TryTakeValue<TKey, TValue>(this IDictionary<TKey, TValue> list, TKey key)
        {
            if (list.ContainsKey(key))
            {
                TValue value = list[key];
                list.Remove(key);
                return value;
            }

            return default;
        }

        /// <summary>
        /// Adds item if key not already in collection
        /// </summary>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> list, TKey key, TValue item)
        {
            if (list.ContainsKey(key))
                return false;

            list.Add(key, item);
            return true;
        }

        /// <summary>
        /// Adds item or sets the value if it exists already
        /// </summary>
        public static void AddOrSet<TKey, TValue>(this IDictionary<TKey, TValue> list, TKey key, TValue value)
        {
            if (list.ContainsKey(key))
                list[key] = value;
            else
                list.Add(key, value);
        }

        public static void Increment<TKey>(this IDictionary<TKey, int> list, TKey key)
        {
            list.Increment(key, 1);
        }

        public static void Increment<TKey>(this IDictionary<TKey, int> list, TKey key, int increment)
        {
            if (list.ContainsKey(key))
                list[key] += increment;
            else
                list.Add(key, increment);
        }

        public static void MergeIn<TKey, TValue>(this IDictionary<TKey, TValue> list, IDictionary<TKey, TValue> other)
        {
            if (other == null)
                return;

            foreach (KeyValuePair<TKey, TValue> kvp in other)
            {
                list.TryAdd(kvp.Key, kvp.Value);
            }
        }

        public static IDictionary<TKey, TValue> AsDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> kvp in keyValues)
            {
                dictionary.TryAdd(kvp.Key, kvp.Value);
            }

            return dictionary;
        }
    }
}