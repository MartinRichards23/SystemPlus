﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SystemPlus.Collections.Generic;

namespace SystemPlus.Threading
{
    /// <summary>
    /// Base class that manages the processing of items using variable number of threads
    /// </summary>
    public abstract class ProcessorBase<T>
    {
        #region Fields

        int maxThreads = 1;
        readonly CancellationToken cancelToken;

        long processedItems;

        readonly object key = new object();
        readonly object syncRoot = new object();
        readonly IProducerConsumerCollection<T> items;
        readonly IList<Task> threads = new List<Task>();

        public event Action StartedWorking;
        public event Action StoppedWorking;
        public event Action<T, Exception> Error;
        public event Action<T> ItemProcessed;

        TimeSpan threadWait = TimeSpan.FromSeconds(1);

        #endregion

        #region Constructors

        protected ProcessorBase()
            : this(Environment.ProcessorCount, CancellationToken.None, new ConcurrentQueue<T>())
        {
        }

        protected ProcessorBase(CancellationToken cancelToken)
            : this(Environment.ProcessorCount, cancelToken, new ConcurrentQueue<T>())
        {
        }

        protected ProcessorBase(int maxThreads)
            : this(maxThreads, CancellationToken.None, new ConcurrentQueue<T>())
        {
        }

        protected ProcessorBase(int maxThreads, IProducerConsumerCollection<T> baseCollection)
            : this(maxThreads, CancellationToken.None, baseCollection)
        {
        }

        protected ProcessorBase(int maxThreads, CancellationToken cancelToken)
            : this(maxThreads, cancelToken, new ConcurrentQueue<T>())
        {
        }

        protected ProcessorBase(int maxThreads, CancellationToken cancelToken, IProducerConsumerCollection<T> baseCollection)
        {
            items = baseCollection;
            MaxThreads = maxThreads;
            this.cancelToken = cancelToken;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Tha max number of concurrent threads to use
        /// </summary>
        public int MaxThreads
        {
            get { return maxThreads; }
            protected set
            {
                if (value < 1)
                    throw new InvalidOperationException("Max threads must be above 0");

                maxThreads = value;
            }
        }

        /// <summary>
        /// The number of threads currently processing
        /// </summary>
        public int ActiveThreads
        {
            get { return threads.Count; }
        }

        /// <summary>
        /// Indicates if there are any active threads
        /// </summary>
        public bool IsActive
        {
            get { return ActiveThreads > 0; }
        }

        /// <summary>
        /// The number of items waiting to be processed
        /// </summary>
        public int ItemCount
        {
            get { return items.Count; }
        }

        /// <summary>
        /// The total number of items processed
        /// </summary>
        public long ProcessedItems
        {
            get { return processedItems; }
        }

        /// <summary>
        /// Token to enable cancellation of workers
        /// </summary>
        protected CancellationToken CancelToken
        {
            get { return cancelToken; }
        }

        /// <summary>
        /// Object for syncronising worker threads
        /// </summary>
        protected object SyncRoot
        {
            get { return syncRoot; }
        }

        /// <summary>
        /// Duration a thread will wait before terminating
        /// </summary>
        public TimeSpan ThreadWait
        {
            get { return threadWait; }
            set { threadWait = value; }
        }

        #endregion

        #region Methods

        protected abstract void ProcessItem(T item);

        /// <summary>
        /// Gets next item to be processed
        /// </summary>
        bool GetNextItem(out T item)
        {
            TimeSpan ts = TimeSpan.Zero;
            TimeSpan gap = TimeSpan.FromMilliseconds(50);

            do
            {
                if (cancelToken.IsCancellationRequested)
                {
                    item = default(T);
                    return false;
                }

                // if we have a result then take it
                if (items.TryTake(out item))
                    return true;

                // otherwise sleep for a bit and try again
                Thread.Sleep(gap);
                ts += gap;
            } while (ts < threadWait);

            return false;
        }

        void DoProcessing()
        {
            while (GetNextItem(out T item))
            {
                if (cancelToken.IsCancellationRequested)
                    break;

                try
                {
                    ProcessItem(item);
                }
                catch (OperationCanceledException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    OnErrorSafe(item, ex);
                }
                finally
                {
                    processedItems++;
                    OnItemProcessed(item);
                }
            }
        }

        void Finish(Task task)
        {
            lock (key)
            {
                // Remove thread from list of active threads
                threads.Remove(task);
            }

            if (threads.Count < 1)
                OnStoppedWorking();
        }

        void StartThread()
        {
            Action action = DoProcessing;
            Action<Task> finish = Finish;

            Task task = new Task(action);
            task.ContinueWith(finish);

            threads.Add(task);
            task.Start();
        }

        /// <summary>
        /// Add items to be processing queue
        /// </summary>
        /// <param name="items">The item to process</param>
        public void Add(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Add item to be processing queue
        /// </summary>
        /// <param name="item">The item to process</param>
        public virtual void Add(T item)
        {
            items.TryAdd(item);

            bool started = false;
            lock (key)
            {
                // if we don't have enough threads running start a new one
                if (threads.Count < maxThreads)
                {
                    if (threads.Count == 0)
                        started = true;

                    StartThread();
                }
            }

            if (started)
                OnStartedWorking();
        }

        /// <summary>
        /// Clears all items without processing them
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Block while all tasks finish
        /// </summary>
        public void Wait()
        {
            Wait(Timeout.Infinite);
        }

        public void Wait(int timeout)
        {
            while (true)
            {
                Task t;
                lock (key)
                {
                    if (threads.Count < 1)
                        return;

                    t = threads[0];
                }

                t.Wait(timeout, default(CancellationToken));
            }
        }

        /// <summary>
        ///  Called when first thread starts
        /// </summary>
        protected virtual void OnStartedWorking()
        {
            StartedWorking?.Invoke();
        }

        protected virtual void OnItemProcessed(T item)
        {
            ItemProcessed?.Invoke(item);
        }

        /// <summary>
        /// Called when last thread finishes
        /// </summary>
        protected virtual void OnStoppedWorking()
        {
            StoppedWorking?.Invoke();
        }

        private void OnErrorSafe(T item, Exception ex)
        {
            try
            {
                OnError(item, ex);
            }
            catch { }
        }

        /// <summary>
        /// Called when processing an item throws an exception
        /// </summary>
        protected virtual void OnError(T item, Exception ex)
        {
            Error?.Invoke(item, ex);
        }

        public override string ToString()
        {
            return string.Format("Items={0}, Processed items={1}, Threads={2}", ItemCount, ProcessedItems, ActiveThreads);
        }

        #endregion
    }
}