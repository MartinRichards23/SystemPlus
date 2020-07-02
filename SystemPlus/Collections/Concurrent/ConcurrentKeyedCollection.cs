using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SystemPlus.Collections.ObjectModel;

namespace SystemPlus.Collections.Concurrent
{
    public class ConcurrentKeyedCollection<T> : IProducerConsumerCollection<T>, ICollection<T> where T : IKeyed
    {
        readonly object key = new object();
        readonly KeyedCollection<T> items = new KeyedCollection<T>();

        public bool TryAdd(T item)
        {
            lock (key)
            {
                return items.TryAdd(item);
            }
        }

        public bool TryTake(out T item)
        {
            lock (key)
            {
                return items.TryTake(out item);
            }
        }

        public T[] ToArray()
        {
            lock (key)
            {
                return items.ToArray();
            }
        }

        public void CopyTo(T[] array, int index)
        {
            lock (key)
            {
                items.CopyTo(array, index);
            }
        }

        public void CopyTo(Array array, int index)
        {
            lock (key)
            {
                ((ICollection)items).CopyTo(array, index);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (key)
            {
                return items.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (key)
            {
                return items.GetEnumerator();
            }
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                lock (key)
                {
                    return items.Count;
                }
            }
        }

        public bool IsSynchronized
        {
            get
            {
                lock (key)
                {
                    return true;
                }
            }
        }

        public object SyncRoot
        {
            get { return key; }
        }

        public bool IsReadOnly { get { return false; } }
    }
}