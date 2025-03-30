using System.Collections.Concurrent;
using System.Windows.Threading;

namespace SystemPlus.Windows
{
    /// <summary>
    /// Helper class for batching up operations to process them in one go
    /// </summary>
    public class BatchInvoker<T>
    {
        #region Fields

        readonly Dispatcher dispatcher;
        readonly Action<IEnumerable<T>> action;
        readonly DispatcherPriority priority;

        readonly ConcurrentQueue<T> waitingToAdd = new ConcurrentQueue<T>();
        bool waiting = true;

        #endregion

        public BatchInvoker(Dispatcher dispatcher, Action<IEnumerable<T>> action)
            : this(dispatcher, action, DispatcherPriority.Loaded)
        {
        }

        public BatchInvoker(Dispatcher dispatcher, Action<IEnumerable<T>> action, DispatcherPriority priority)
        {
            this.dispatcher = dispatcher;
            this.action = action;
            this.priority = priority;
        }

        #region Properties

        public DispatcherPriority Priority
        {
            get { return priority; }
        }

        #endregion

        #region Methods

        public void Add(T item)
        {
            waitingToAdd.Enqueue(item);

            Process();
        }

        public void Add(IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (T item in items)
                {
                    waitingToAdd.Enqueue(item);
                }
            }

            Process();
        }

        void Process()
        {
            if (waiting)
            {
                waiting = false;

                // add all entities waiting to be added
                dispatcher.BeginInvoke((Action)delegate
                {
                    IList<T> newItems = new List<T>();

                    while (waitingToAdd.TryDequeue(out T? item))
                    {
                        newItems.Add(item);
                    }

                    waiting = true;

                    action(newItems);
                },
                    priority);
            }
        }

        #endregion
    }
}