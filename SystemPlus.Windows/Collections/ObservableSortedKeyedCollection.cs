using System;
using System.Collections.Generic;
using SystemPlus.Collections.ObjectModel;

namespace SystemPlus.Windows.Collections
{
    /// <summary>
    /// Maintains a sorted, keyed, observable collection
    /// </summary>
    public abstract class ObservableSortedKeyedCollection<TKey, TItem> : ObservableKeyedCollection<TKey, TItem>
    {
        protected virtual IComparer<TKey> KeyComparer
        {
            get { return Comparer<TKey>.Default; }
        }

        public IComparer<TItem>? ItemComparer { get; set; }

        protected override void InsertItem(int index, TItem item)
        {
            int insertIndex = index;

            for (int i = 0; i < Count; i++)
            {
                TItem retrievedItem = this[i];

                // if item is icomparable then use that, otherwise sort by key
                if (ItemComparer != null)
                {
                    int val = ItemComparer.Compare(item, retrievedItem);

                    if (val < 0)
                    {
                        insertIndex = i;
                        break;
                    }
                }
                else if (item is IComparable a && retrievedItem is IComparable b)
                {
                    if (a.CompareTo(b) < 0)
                    {
                        insertIndex = i;
                        break;
                    }
                }
                else
                {
                    TKey ak = GetKeyForItem(item);
                    TKey bk = GetKeyForItem(retrievedItem);

                    if (KeyComparer.Compare(ak, bk) < 0)
                    {
                        insertIndex = i;
                        break;
                    }
                }
            }

            base.InsertItem(insertIndex, item);
        }
    }

    [Serializable]
    public class ObservableSortedKeyedCollection<TItem> : ObservableSortedKeyedCollection<string, TItem> where TItem : IKeyed
    {
        protected override string GetKeyForItem(TItem item)
        {
            return item.Key;
        }
    }

    [Serializable]
    public class ObservableSortedKeyedGuidCollection<TItem> : ObservableSortedKeyedCollection<Guid, TItem> where TItem : IGuid
    {
        protected override Guid GetKeyForItem(TItem item)
        {
            return item.Guid;
        }
    }

    [Serializable]
    public class ObservableSortedKeyedIntCollection<TItem> : ObservableSortedKeyedCollection<int, TItem> where TItem : IKeyedInt
    {
        protected override int GetKeyForItem(TItem item)
        {
            return item.Key;
        }
    }

}