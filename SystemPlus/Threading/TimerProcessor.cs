using System;
using System.Threading;

namespace SystemPlus.Threading
{
    /// <summary>
    /// Base class for objects that need a function to run at specific time intervals
    /// </summary>
    public abstract class TimerProcessor : IDisposable
    {
        #region Fields

        Timer timer;
        readonly object key = new object();
        protected CancellationTokenSource CancelToken { get; } = new CancellationTokenSource();

        #endregion

        public void Start(TimeSpan period)
        {
            Start(period, period);
        }

        public void Start(TimeSpan period, TimeSpan dueTime)
        {
            TimerCallback callback = new TimerCallback(OnTimer);
            timer = new Timer(callback, null, dueTime, period);
        }

        public void Stop()
        {
            CancelToken.Cancel();

            // let any existing thread finish
            if (!Monitor.TryEnter(key, 60000))
                throw new Exception("Failed to acquire lock before timeout");

            try
            {
                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }
            }
            finally
            {
                Monitor.Exit(key);
            }
        }

        private void OnTimer(object state)
        {
            if (!Monitor.TryEnter(key))
            {
                // still running previous so return
                return;
            }

            try
            {
                if (CancelToken.IsCancellationRequested)
                    return;

                OnTimer();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
            finally
            {
                Monitor.Exit(key);
            }
        }

        protected abstract void OnTimer();

        public void RunNow()
        {
            OnTimer(null);
        }

        protected virtual void OnError(Exception ex)
        { }

        public void Dispose()
        {
            Stop();
        }
    }
}
