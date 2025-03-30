namespace SystemPlus.Collections.Generic
{
    public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>> where TKey : notnull
    {
        public void Add(TKey key, TValue value)
        {
            KeyValuePair<TKey, TValue> kvp = new KeyValuePair<TKey, TValue>(key, value);
            Add(kvp);
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

            foreach (var kvp in this)
            {
                dictionary.TryAdd(kvp.Key, kvp.Value);
            }

            return dictionary;
        }
    }
}
