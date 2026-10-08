using System.Collections;

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
            ArgumentOutOfRangeException.ThrowIfNegative(maxSize);

            if (items.Count > maxSize)
                items.RemoveRange(0, items.Count - maxSize);
        }

        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Enumerates items in LIFO order (newest first), like Stack&lt;T&gt;
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                yield return items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
