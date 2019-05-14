using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SystemPlus.Collections.Concurrent
{
    public static class Extensions
    {
        public static IEnumerable<T> DequeueAll<T>(this ConcurrentQueue<T> queue)
        {
            List<T> items = new List<T>();

            T item;
            while(queue.TryDequeue(out item))
            {
                items.Add(item);
            }

            return items;           
        }
    }
}
