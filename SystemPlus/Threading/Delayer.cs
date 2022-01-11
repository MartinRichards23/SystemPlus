using System.Threading;

namespace SystemPlus.Threading
{
    /// <summary>
    /// Class that ensures a minimum delay
    /// </summary>
    public class Delayer
    {
        #region Fields

        readonly Random? rand;
        DateTime lastWait = DateTime.MinValue;

        #endregion

        public Delayer(TimeSpan delay)
        {
            MinDelay = delay;
            MaxDelay = delay;
        }

        public Delayer(TimeSpan minDelay, TimeSpan maxDelay)
        {
            MinDelay = minDelay;
            MaxDelay = maxDelay;
            rand = new Random();
        }

        #region Properties

        public TimeSpan MinDelay { get; }
        public TimeSpan MaxDelay { get; }

        #endregion

        /// <summary>
        /// Delays for a minimum time since this method was last called.
        /// </summary>
        public async Task Delay(CancellationToken token = default)
        {
            TimeSpan d;

            if (rand != null)
                d = rand.NextTimespan(MinDelay, MaxDelay);
            else
                d = MinDelay;

            DateTime now = DateTime.UtcNow;

            if (lastWait + d < now)
            {
                TimeSpan duration = now - lastWait;

                await Task.Delay(duration, token);
            }

            lastWait = DateTime.UtcNow;
        }
    }
}