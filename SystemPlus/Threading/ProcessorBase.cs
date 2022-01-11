using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace SystemPlus.Threading
{
    /// <summary>
    /// Base class that manages the processing of items using variable number of threads
    /// </summary>
    public abstract class ProcessorBase<T>
    {
        #region Fields

        int maxThreads = 1;
        readonly object key = new object();
        readonly IProducerConsumerCollection<T> items;
        readonly IList<Task> threads = new List<Task>();

        public event Action? StartedWorking;
        public event Action? StoppedWorking;
        public event Action<T, Exception>? Error;
        public event Action<T>? ItemProcessed;

        TimeSpan threadWait = TimeSpan.FromSeconds(1);

        #endregion

        #region Constructors

        protected ProcessorBase()
            : this(Environment.ProcessorCount, new ConcurrentQueue<T>(), CancellationToken.None)
        {
        }

        protected ProcessorBase(CancellationToken cancelToken)
            : this(Environment.ProcessorCount, new ConcurrentQueue<T>(), cancelToken)
        {
        }

        protected ProcessorBase(int maxThreads)
            : this(maxThreads, new ConcurrentQueue<T>(), CancellationToken.None)
        {
        }

        protected ProcessorBase(int maxThreads, IProducerConsumerCollection<T> baseCollection)
            : this(maxThreads, baseCollection, CancellationToken.None)
        {
        }

        protected ProcessorBase(int maxThreads, CancellationToken cancelToken)
            : this(maxThreads, new ConcurrentQueue<T>(), cancelToken)
        {
        }

        protected ProcessorBase(int maxThreads, IProducerConsumerCollection<T> baseCollection, CancellationToken cancelToken)
        {
            items = baseCollection;
            MaxThreads = maxThreads;
            CancelToken = cancelToken;
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
        public long ProcessedItems { get; private set; }

        /// <summary>
        /// Token to enable cancellation of workers
        /// </summary>
        protected CancellationToken CancelToken { get; }

        /// <summary>
        /// Object for syncronising worker threads
        /// </summary>
        protected object SyncRoot { get; } = new object();

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
        bool GetNextItem([MaybeNullWhen(false)] out T item)
        {
            TimeSpan ts = TimeSpan.Zero;
            TimeSpan gap = TimeSpan.FromMilliseconds(50);

            do
            {
                if (CancelToken.IsCancellationRequested)
                {
                    item = default;
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
            while (GetNextItem(out T? item))
            {
                if (CancelToken.IsCancellationRequested)
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
                    ProcessedItems++;
                    OnItemFinished(item);
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
            task.ContinueWith(finish, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);

            threads.Add(task);
            task.Start();
        }

        /// <summary>
        /// Add items to be processing queue
        /// </summary>
        /// <param name="items">The item to process</param>
        public void Add(IEnumerable<T> items)
        {
            if (items == null)
                return;

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
            while (items.TryTake(out T? item))
            {
                OnItemFinished(item);
            }
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

                t.Wait(timeout, default);
            }
        }

        /// <summary>
        ///  Called when first thread starts
        /// </summary>
        protected virtual void OnStartedWorking()
        {
            StartedWorking?.Invoke();
        }

        protected virtual void OnItemFinished(T item)
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
            return $"Items={ItemCount}, Processed items={ProcessedItems}, Threads={ActiveThreads}";
        }

        #endregion
    }
}