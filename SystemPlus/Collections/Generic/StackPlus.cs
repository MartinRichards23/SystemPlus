using System.Collections;
using System.Collections.Generic;

namespace SystemPlus.Collections.Generic
{
    /// <summary>
    /// A Stack implementation with extra funcationality
    /// </summary>
    public class StackPlus<T> : IEnumerable<T>
    {
        readonly List<T> items = new List<T>();

        public int Count { get { return items.Count; } }

        public void Push(T item)
        {
            items.Add(item);
        }

        public T? Pop()
        {
            if (items.Count > 0)
            {
                int index = items.Count - 1;
                return Remove(index);
            }

            return default;
        }

        public T Remove(int index)
        {
            T item = items[index];
            items.RemoveAt(index);

            return item;
        }

        public void RemoveOldest(int maxSize)
        {
            while (items.Count > maxSize)
            {
                items.RemoveAt(0);
            }
        }

        public void Clear()
        {
            items.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }
    }
}
