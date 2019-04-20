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
        IComparer<TItem> itemComparer;

        protected virtual IComparer<TKey> KeyComparer
        {
            get { return Comparer<TKey>.Default; }
        }

        public IComparer<TItem> ItemComparer
        {
            get { return itemComparer; }
            set { itemComparer = value; }
        }

        protected override void InsertItem(int index, TItem item)
        {
            int insertIndex = index;

            for (int i = 0; i < Count; i++)
            {
                TItem retrievedItem = this[i];

                // if item is icomparable then use that, otherwise sort by key
                if (itemComparer != null)
                {
                    int val = itemComparer.Compare(item, retrievedItem);

                    if (val < 0)
                    {
                        insertIndex = i;
                        break;
                    }
                }
                else if (item is IComparable)
                {
                    IComparable a = (IComparable)item;
                    IComparable b = (IComparable)retrievedItem;

                    if (a.CompareTo(b) < 0)
                    {
                        insertIndex = i;
                        break;
                    }
                }
                else
                {
                    TKey a = GetKeyForItem(item);
                    TKey b = GetKeyForItem(retrievedItem);

                    if (KeyComparer.Compare(a, b) < 0)
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