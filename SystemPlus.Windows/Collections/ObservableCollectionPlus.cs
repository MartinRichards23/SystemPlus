using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;

namespace SystemPlus.Windows.Collections
{
    /// <summary>
    /// An implementation of ObservableCollection that provides suspend and resume functionality
    /// and can be updated from any thread
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableCollectionPlus<T> : ObservableCollection<T>
    {
        readonly Dispatcher dispatcher;
        bool isSuspended;

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
            this.dispatcher = dispatcher;

            foreach (T item in collection)
            {
                Add(item);
            }
        }

        #endregion

        /// <summary>
        /// Gets whether CollectionChanged events are currently suspended
        /// </summary>
        public bool IsSuspended
        {
            get { return isSuspended; }
            private set { isSuspended = value; }
        }

        public Dispatcher Dispatcher
        {
            get { return dispatcher; }
        }

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
                foreach (T item in items)
                {
                    Add(item);
                }
            }
            finally
            {
                // resume if it was previously in resumed state
                if (!origIsSuspended)
                    Resume();
            }
        }

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
            Dispatcher.Invoke((Action)(() => base.InsertItem(index, item)));
        }

        protected override void SetItem(int index, T item)
        {
            Dispatcher.Invoke((Action)(() => base.SetItem(index, item)));
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            Dispatcher.Invoke((Action)(() => base.MoveItem(oldIndex, newIndex)));
        }

        protected override void RemoveItem(int index)
        {
            Dispatcher.Invoke((Action)(() => base.RemoveItem(index)));
        }

        protected override void ClearItems()
        {
            Dispatcher.Invoke((Action)(() => base.ClearItems()));
        }
    }
}