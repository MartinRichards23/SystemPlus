using System.Collections.ObjectModel;
using System.Collections.Specialized;

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

        public static void ProcessChangedEvent<T>(this ObservableCollection<T> target, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        target.Insert(e.NewStartingIndex + i, (T)e.NewItems[i]);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldItems.Count == 1)
                    {
                        target.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else
                    {
                        List<T> items = target.Skip(e.OldStartingIndex).Take(e.OldItems.Count).ToList();
                        for (int i = 0; i < e.OldItems.Count; i++)
                            target.RemoveAt(e.OldStartingIndex);

                        for (int i = 0; i < items.Count; i++)
                            target.Insert(e.NewStartingIndex + i, items[i]);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++)
                        target.RemoveAt(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    // remove
                    for (int i = 0; i < e.OldItems.Count; i++)
                        target.RemoveAt(e.OldStartingIndex);

                    // add
                    goto case NotifyCollectionChangedAction.Add;

                case NotifyCollectionChangedAction.Reset:
                    target.Clear();

                    for (int i = 0; i < e.NewItems.Count; i++)
                        target.Add((T)e.NewItems[i]);

                    break;

                default:
                    break;
            }
        }
    }
}