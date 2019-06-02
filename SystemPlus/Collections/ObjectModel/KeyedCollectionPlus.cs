using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SystemPlus.Collections.ObjectModel
{
    /// <summary>
    /// Adds extra functionality to KeyedCollection
    /// </summary>
    [Serializable]
    public abstract class KeyedCollectionPlus<TKey, TItem> : KeyedCollection<TKey, TItem>
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
            foreach (TItem item in items)
            {
                Add(item);
            }
        }

        public void TryAddRange(IEnumerable<TItem> items)
        {
            foreach (TItem item in items)
            {
                TryAdd(item);
            }
        }

        public TItem ItemOrDefault(TKey key)
        {
            if (Contains(key))
                return this[key];

            return default(TItem);
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
            return string.Format("Type: {0} Count: {1}", typeof(TItem).Name, Count);
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

        public TItem TryGet(TKey key)
        {
            if (Contains(key))
                return this[key];

            return default(TItem);
        }

        /// <summary>
        /// Attempts to take the first item
        /// </summary>
        public bool TryTake(out TItem item)
        {
            if (Count > 0)
            {
                item = this[0];
                RemoveAt(0);
                return true;
            }

            item = default(TItem);
            return false;
        }
    }
}