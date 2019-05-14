using System;
using System.Threading;
using System.Threading.Tasks;

namespace SystemPlus.Threading
{
    /// <summary>
    /// Class that ensures Wait() is called no more frequently than set by the Delay
    /// </summary>
    public class Delayer
    {
        readonly TimeSpan minDelay;
        readonly TimeSpan maxDelay;
        readonly Random rand;

        DateTime lastWait = DateTime.MinValue;

        public Delayer(TimeSpan delay)
        {
            minDelay = delay;
            maxDelay = delay;
        }

        public Delayer(TimeSpan minDelay, TimeSpan maxDelay)
        {
            this.minDelay = minDelay;
            this.maxDelay = maxDelay;
            rand = new Random();
        }

        public TimeSpan MinDelay
        {
            get { return minDelay; }
        }

        public TimeSpan MaxDelay
        {
            get { return maxDelay; }
        }

        public void Wait(CancellationToken token = default(CancellationToken))
        {
            TimeSpan d;

            if (rand != null)
                d = rand.NextTimespan(minDelay, maxDelay);
            else
                d = minDelay;

            while (true)
            {
                TimeSpan duration = DateTime.UtcNow - lastWait;

                if (duration > d)
                {
                    // have waited long enough
                    break;
                }

                Thread.Sleep(50);
                //Task.Delay(TimeSpan.FromMilliseconds(50), token).Wait();
            }

            lastWait = DateTime.UtcNow;
        }
    }
}