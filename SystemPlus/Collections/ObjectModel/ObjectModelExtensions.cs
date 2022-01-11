namespace SystemPlus.Collections.ObjectModel
{
    public static class ObjectModelExtensions
    {
        public static KeyedCollection<T> ToKeyedCollection<T>(this IEnumerable<T> list) where T : IKeyed
        {
            KeyedCollection<T> items = new KeyedCollection<T>();
            items.AddRange(list);

            return items;
        }

        public static KeyedIntCollection<T> ToKeyedIntCollection<T>(this IEnumerable<T> list) where T : IKeyedInt
        {
            KeyedIntCollection<T> items = new KeyedIntCollection<T>();
            items.AddRange(list);

            return items;
        }

        public static KeyedGuidCollection<T> ToKeyedGuidCollection<T>(this IEnumerable<T> list) where T : IGuid
        {
            KeyedGuidCollection<T> items = new KeyedGuidCollection<T>();
            items.AddRange(list);

            return items;
        }
    }
}