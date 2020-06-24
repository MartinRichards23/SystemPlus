using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SystemPlus.Collections.Concurrent
{
    public static class ConcurrentExtensions
    {
        public static IEnumerable<T> DequeueAll<T>(this ConcurrentQueue<T> queue)
        {
            List<T> items = new List<T>();

            while (queue.TryDequeue(out T item))
            {
                items.Add(item);
            }

            return items;
        }
    }
}
