using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace SystemPlus.Windows.Collections
{
    /// <summary>
    /// An implementation of ObservableCollection that provides suspend and resume functionality and can be updated from any thread
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableCollectionPlus<T> : ObservableCollection<T>
    {
        #region Constructors

        public ObservableCollectionPlus()
            : this(Enumerable.Empty<T>())
        {
        }

        public ObservableCollectionPlus(Dispatcher dispatcher)
            : this(Enumerable.Empty<T>(), dispatcher)
        {
        }

        public ObservableCollectionPlus(IEnumerable<T> collection)
            : this(collection, Dispatcher.CurrentDispatcher)
        {
        }

        public ObservableCollectionPlus(IEnumerable<T> collection, Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;

            if (collection != null)
            {
                foreach (T item in collection)
                {
                    Add(item);
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether CollectionChanged events are currently suspended
        /// </summary>
        public bool IsSuspended { get; private set; }

        public Dispatcher Dispatcher { get; }

        #endregion

        #region Public methods

        /// <summary>
        /// Suspends CollectionChanged events
        /// </summary>
        public void Suspend()
        {
            IsSuspended = true;
        }

        /// <summary>
        /// Resumes CollectionChanged events
        /// </summary>
        public void Resume()
        {
            if (IsSuspended)
            {
                IsSuspended = false;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            bool origIsSuspended = IsSuspended;
            Suspend();

            try
            {
                if (items != null)
                {
                    foreach (T item in items)
                    {
                        Add(item);
                    }
                }
            }
            finally
            {
                // resume if it was previously in resumed state
                if (!origIsSuspended)
                    Resume();
            }
        }

        #endregion

        #region Protected methods

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (IsSuspended)
                return;

            base.OnCollectionChanged(e);

            //Dispatcher.InvokeIfNeeded(() => base.OnCollectionChanged(e));
        }

        //protected override void OnPropertyChanged(ComponentModel.PropertyChangedEventArgs e)
        //{
        //    Dispatcher.InvokeIfNeeded(() => base.OnPropertyChanged(e));
        //}

        protected override void InsertItem(int index, T item)
        {
            Dispatcher.Invoke(() => base.InsertItem(index, item));
        }

        protected override void SetItem(int index, T item)
        {
            Dispatcher.Invoke(() => base.SetItem(index, item));
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            Dispatcher.Invoke(() => base.MoveItem(oldIndex, newIndex));
        }

        protected override void RemoveItem(int index)
        {
            Dispatcher.Invoke(() => base.RemoveItem(index));
        }

        protected override void ClearItems()
        {
            Dispatcher.Invoke(() => base.ClearItems());
        }

        #endregion
    }
}