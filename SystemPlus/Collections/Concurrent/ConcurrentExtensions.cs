using System.Collections.Concurrent;

namespace SystemPlus.Collections.Concurrent
{
    public static class ConcurrentExtensions
    {
        public static IEnumerable<T> DequeueAll<T>(this ConcurrentQueue<T> queue)
        {
            ArgumentNullException.ThrowIfNull(queue);

            List<T> items = new List<T>();

            while (queue.TryDequeue(out T? item))
            {
                items.Add(item);
            }

            return items;
        }
    }
}
