using System.Diagnostics.CodeAnalysis;

namespace SystemPlus.Collections.Generic
{
    /// <summary>
    /// Extension methods for Dictionaries
    /// </summary>
    public static class DictionaryExtensions
    {
        public static void MergeIn<TKey, TValue>(this IDictionary<TKey, TValue> list, IDictionary<TKey, TValue> other) where TKey : notnull
        {
            other.ThrowIfNull(nameof(other));
            
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