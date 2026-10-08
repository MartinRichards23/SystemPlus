using System.Diagnostics;
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
        readonly SemaphoreSlim gate = new SemaphoreSlim(1, 1);
        long? lastWaitTicks;

        #endregion

        public Delayer(TimeSpan delay)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(delay, TimeSpan.Zero);

            MinDelay = delay;
            MaxDelay = delay;
        }

        public Delayer(TimeSpan minDelay, TimeSpan maxDelay)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(minDelay, TimeSpan.Zero);
            ArgumentOutOfRangeException.ThrowIfLessThan(maxDelay, minDelay);

            MinDelay = minDelay;
            MaxDelay = maxDelay;
            rand = Random.Shared;
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
            await gate.WaitAsync(token).ConfigureAwait(false);

            try
            {
                TimeSpan d;

                if (rand != null)
                    d = rand.NextTimespan(MinDelay, MaxDelay);
                else
                    d = MinDelay;

                if (lastWaitTicks.HasValue)
                {
                    TimeSpan elapsed = Stopwatch.GetElapsedTime(lastWaitTicks.Value);

                    if (elapsed < d)
                        await Task.Delay(d - elapsed, token).ConfigureAwait(false);
                }
            }
            finally
            {
                lastWaitTicks = Stopwatch.GetTimestamp();
                gate.Release();
            }
        }
    }
}