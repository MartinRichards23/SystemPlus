using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace SystemPlus.Collections.ObjectModel
{
    /// <summary>
    /// Adds extra functionality to KeyedCollection
    /// </summary>
    [Serializable]
    public abstract class KeyedCollectionPlus<TKey, TItem> : KeyedCollection<TKey, TItem> where TKey : notnull
    {
        public virtual void Sort()
        {
            List<TItem> list = (List<TItem>)Items;
            list.Sort();
        }

        public virtual void Sort(Comparison<TItem> comparer)
        {
            List<TItem> list = (List<TItem>)Items;
            list.Sort(comparer);
        }

        public virtual void Sort(Comparer<TItem> comparer)
        {
            List<TItem> list = (List<TItem>)Items;
            list.Sort(comparer);
        }

        public void AddRange(IEnumerable<TItem> items)
        {
            if (items != null)
            {
                foreach (TItem item in items)
                {
                    Add(item);
                }
            }
        }

        public void TryAddRange(IEnumerable<TItem> items)
        {
            if (items != null)
            {
                foreach (TItem item in items)
                {
                    TryAdd(item);
                }
            }
        }

        [return: MaybeNull]
        public TItem ItemOrDefault(TKey key)
        {
            if (Contains(key))
                return this[key];

            return default;
        }

        /// <summary>
        /// Removes a values and returns it
        /// </summary>
        public TItem TakeValue(TKey key)
        {
            TItem value = this[key];
            Remove(key);
            return value;
        }

        public override string ToString()
        {
            return $"Type: {typeof(TItem).Name} Count: {Count}";
        }

        /// <summary>
        /// Adds an item if no item with same key exists already
        /// </summary>
        public bool TryAdd(TItem item)
        {
            TKey itemKey = GetKeyForItem(item);

            if (itemKey == null)
                return false;

            if (Contains(itemKey))
                return false;

            Add(item);
            return true;
        }

        /// <summary>
        /// Adds an item, if item with same key exists already it is removed first
        /// </summary>
        public void AddOrReplace(TItem item)
        {
            TKey itemKey = GetKeyForItem(item);

            if (Contains(itemKey))
                Remove(itemKey);

            Add(item);
        }

        [return: MaybeNull]
        public TItem TryGet(TKey key)
        {
            if (Contains(key))
                return this[key];

            return default;
        }

        public bool TryGet(TKey key, [MaybeNullWhen(false)] out TItem value)
        {
            if (Contains(key))
            {
                value = this[key];
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Attempts to take the first item
        /// </summary>
        public bool TryTake([MaybeNullWhen(false)] out TItem item)
        {
            if (Count > 0)
            {
                item = this[0];
                RemoveAt(0);
                return true;
            }

            item = default;
            return false;
        }
    }
}