/*using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;

namespace SystemPlus.ComponentModel
{
    /// <summary>
    /// Manages progress tokens
    /// </summary>
    public class ProgressManager : NotifyPropertyChanged
    {
        #region Fields

        readonly Dispatcher dispatcher;
        readonly ObservableCollection<IProgressToken> tokens;
        bool isActive;

        public event EventHandler AllFinished;

        #endregion

        public ProgressManager(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            tokens = new ObservableCollection<IProgressToken>();
            tokens.CollectionChanged += tokens_CollectionChanged;
        }

        #region Properties

        public ObservableCollection<IProgressToken> Tokens
        {
            get { return tokens; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    OnPropertyChanged("IsActive");
                }
            }
        }

        public int Count
        {
            get { return tokens.Count; }
        }

        #endregion

        public IProgressToken MakeToken(string title)
        {
            return MakeToken<ProgressToken>(title);
        }

        public T MakeToken<T>(string title) where T : IProgressToken, new()
        {
            T token = new T();
            token.Title = title;
            token.Cancelled += token_Cancelled;

            dispatcher.Invoke((Action)(() => tokens.Add(token)));

            return token;
        }

        public void RemoveToken(IProgressToken token)
        {
            token.Cancelled -= token_Cancelled;
            dispatcher.Invoke((Action)(() => tokens.Remove(token)));
        }

        public void CancelAll()
        {
            foreach (IProgressToken token in tokens.ToArray())
            {
                token.Cancel();
            }
        }

        void tokens_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsActive = Count > 0;

            if (Count <= 0)
                OnAllFinished();

            OnPropertyChanged("Count");
        }

        void token_Cancelled(IProgressToken token)
        {
            // token cancelled, so remove it
            RemoveToken(token);
        }

        protected void OnAllFinished()
        {
            if (AllFinished != null)
                AllFinished(this, null);
        }
    }
}*/