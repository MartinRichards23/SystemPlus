namespace SystemPlus.Collections.ObjectModel
{
    [Serializable]
    public class KeyValueCollection<TKey, TValue> : KeyedCollectionPlus<TKey, KeyValuePair<TKey, TValue>> where TKey : notnull
    {
        protected override TKey GetKeyForItem(KeyValuePair<TKey, TValue> item)
        {
            return item.Key;
        }
    }

    [Serializable]
    public class KeyedCollection<TItem> : KeyedCollectionPlus<string, TItem> where TItem : IKeyed
    {
        protected override string GetKeyForItem(TItem item)
        {
            return item.Key;
        }
    }

    [Serializable]
    public class KeyedGuidCollection<TItem> : KeyedCollectionPlus<Guid, TItem> where TItem : IGuid
    {
        protected override Guid GetKeyForItem(TItem item)
        {
            return item.Guid;
        }
    }

    [Serializable]
    public class KeyedIntCollection<TItem> : KeyedCollectionPlus<int, TItem> where TItem : IKeyedInt
    {
        protected override int GetKeyForItem(TItem item)
        {
            return item.Key;
        }
    }

    public interface IGuid
    {
        Guid Guid { get; }
    }

    public interface IKeyed
    {
        string Key { get; }
    }

    public interface IKeyedInt
    {
        int Key { get; }
    }
}