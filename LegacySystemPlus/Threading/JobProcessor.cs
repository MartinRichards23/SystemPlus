using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SystemPlus.Threading
{
    public class JobProcessor : IDisposable
    {
        #region Fields

        Timer timer;
        protected CancellationTokenSource CancelToken { get; } = new CancellationTokenSource();

        readonly List<JobItem> jobs = new List<JobItem>();
        public event Action<Exception, ITimed> Error;

        #endregion

        public IEnumerable<ITimed> Jobs
        {
            get { return jobs.Select(j => j.Job); }
        }

        public void Start()
        {
            Start(TimeSpan.FromSeconds(15));
        }

        public void Start(TimeSpan checkFrequency)
        {
            TimerCallback callback = new TimerCallback(OnTimer);
            timer = new Timer(callback, null, checkFrequency, checkFrequency);
        }

        public void Stop()
        {
            CancelToken.Cancel();

            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        public void AddJob(ITimed job, TimeSpan interval)
        {
            AddJob(job, interval, interval);
        }

        public void AddJob(ITimed job, TimeSpan interval, TimeSpan initialDelay)
        {
            JobItem item = new JobItem(job, interval, initialDelay);

            jobs.Add(item);
        }

        public bool RemoveJob(ITimed job)
        {
            JobItem item = jobs.FirstOrDefault(j => j.Job == job);

            if (item != null)
                return jobs.Remove(item);

            return false;
        }

        private void OnTimer(object state)
        {
            foreach (JobItem item in jobs)
            {
                if (CancelToken.IsCancellationRequested)
                    break;

                if(item.NextRun > DateTime.UtcNow)
                {
                    // not ready to run yet
                    continue;
                }

                try
                {
                    item.RunNow();
                }
                catch (Exception ex)
                {
                    if (Error != null)
                        Error(ex, item.Job);
                }
            }
        }

        public void Dispose()
        {
            Stop();
        }

        class JobItem
        {
            public JobItem(ITimed job, TimeSpan interval, TimeSpan initialDelay)
            {
                Job = job;
                Interval = interval;
                NextRun = DateTime.UtcNow + initialDelay;
            }
            
            public ITimed Job { get; }
            public TimeSpan Interval { get; }
            public DateTime NextRun { get; set; }

            public void RunNow()
            {
                NextRun = DateTime.UtcNow + Interval;

                Job.OnTimer();
            }
        }
    }

    public interface ITimed
    {
        void OnTimer();
    }
}
