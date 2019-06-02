using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;
using SystemPlus.Collections.ObjectModel;

namespace SystemPlus.Windows.Collections
{
    /// <summary>
    /// Extends KeyedCollectionPlus to become observable
    /// </summary>
    [Serializable]
    public abstract class ObservableKeyedCollection<TKey, TItem> : KeyedCollectionPlus<TKey, TItem>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Fields

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        [NonSerialized]
        readonly Dispatcher dispatcher;

        bool isSuspended;

        #endregion

        protected ObservableKeyedCollection()
            : this(Dispatcher.CurrentDispatcher)
        {
        }

        protected ObservableKeyedCollection(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        #region Properties

        public Dispatcher Dispatcher
        {
            get { return dispatcher; }
        }

        /// <summary>
        /// Gets whether CollectionChanged events are currently suspended
        /// </summary>
        public bool IsSuspended
        {
            get { return isSuspended; }
            private set { isSuspended = value; }
        }

        #endregion

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

        public void InvokeAdd(TItem item)
        {
            Dispatcher.Invoke((Action)(() => Add(item)));
        }

        public void InvokeAddRange(IEnumerable<TItem> items)
        {
            Dispatcher.Invoke((Action)(() => AddRange(items)));
        }

        public void InvokeRemove(TItem item)
        {
            Dispatcher.Invoke((Action)(() => Remove(item)));
        }

        public void InvokeRemove(TKey key)
        {
            Dispatcher.Invoke((Action)(() => Remove(key)));
        }

        public void InvokeClear()
        {
            Dispatcher.Invoke((Action)(Clear));
        }

        protected override void InsertItem(int index, TItem item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        protected override void SetItem(int index, TItem item)
        {
            base.SetItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, index));
        }

        protected override void RemoveItem(int index)
        {
            TItem item = this[index];
            base.RemoveItem(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs arg)
        {
            if (IsSuspended)
                return;

            if (CollectionChanged != null)
                CollectionChanged(this, arg);
        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public override void Sort()
        {
            base.Sort();

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public override void Sort(Comparison<TItem> comparer)
        {
            base.Sort(comparer);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    [Serializable]
    public class ObservableKeyedCollection<TItem> : ObservableKeyedCollection<string, TItem> where TItem : IKeyed
    {
        protected override string GetKeyForItem(TItem item)
        {
            return item.Key;
        }
    }

    [Serializable]
    public class ObservableKeyedGuidCollection<TItem> : ObservableKeyedCollection<Guid, TItem> where TItem : IGuid
    {
        protected override Guid GetKeyForItem(TItem item)
        {
            return item.Guid;
        }
    }

    [Serializable]
    public class ObservableKeyedIntCollection<TItem> : ObservableKeyedCollection<int, TItem> where TItem : IKeyedInt
    {
        protected override int GetKeyForItem(TItem item)
        {
            return item.Key;
        }
    }
}