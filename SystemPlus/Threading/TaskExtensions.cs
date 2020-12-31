using System;
using System.Threading;
using System.Threading.Tasks;

namespace SystemPlus.Threading
{
    /// <summary>
    /// Extensions for System.Threading.Tasks
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Await a Task with a timeout
        /// </summary>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            using CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource();

            Task completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));

            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                return await task;  // propagate exceptions
            }
            else
            {
                throw new TimeoutException();
            }
        }

        /// <summary>
        /// Allows an async function to be called and not awaited without the warning.
        /// </summary>
        public static void DoNotAwait(this Task _) { }
    }
}
