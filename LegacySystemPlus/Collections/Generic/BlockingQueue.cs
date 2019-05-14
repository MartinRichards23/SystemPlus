using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace SystemPlus.Collections.Generic
{
    /// <summary>
    /// A simple blocking queue that can be used in a FIFO or FILO way
    /// </summary>
    public class BlockingQueue<T> : IEnumerable<T>
    {
        bool closing;
        readonly IList<T> list = new List<T>();

        public void Enqueue(T data)
        {
            lock (list)
            {
                list.Add(data);
                Monitor.Pulse(list);
            }
        }

        public bool TryTake(out T value)
        {
            lock (list)
            {
                while (list.Count == 0)
                {
                    if (closing)
                    {
                        value = default(T);
                        return false;
                    }

                    Monitor.Wait(list);
                }

                value = list[0];
                list.RemoveAt(0);

                return true;
            }
        }

        public void EnqueueFirst(T data)
        {
            lock (list)
            {
                list.Insert(0, data);
                Monitor.Pulse(list);
            }
        }

        public bool TryTakeLast(out T value)
        {
            lock (list)
            {
                while (list.Count == 0)
                {
                    if (closing)
                    {
                        value = default(T);
                        return false;
                    }

                    Monitor.Wait(list);
                }

                int index = list.Count - 1;
                value = list[index];
                list.RemoveAt(index);

                return true;
            }
        }

        public void Close()
        {
            lock (list)
            {
                closing = true;
                Monitor.PulseAll(list);
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            T item;
            while (TryTakeLast(out item))
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        public void Add(T item)
        {
            Enqueue(item);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public override string ToString()
        {
            return "Count: " + Count.ToString(CultureInfo.InvariantCulture);
        }
    }
}