using System.Threading;

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
            using (CancellationTokenSource timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                Task completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));

                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;  // propagate exceptions
                }
                else
                {
                    // observe any later fault so it isn't reported as an unobserved task exception
                    _ = task.ContinueWith(t => _ = t.Exception, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                    throw new TimeoutException();
                }
            }
        }

        /// <summary>
        /// Await a Task that doesn't support cancellation, throwing OperationCanceledException as soon as the token is cancelled.
        /// The underlying task keeps running; its result and any exception are discarded.
        /// </summary>
        public static async Task<TResult> WithCancellation<TResult>(this Task<TResult> task, CancellationToken cancelToken)
        {
            if (!cancelToken.CanBeCanceled)
                return await task;

            TaskCompletionSource<bool> cancelled = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (cancelToken.Register(() => cancelled.TrySetResult(true)))
            {
                if (await Task.WhenAny(task, cancelled.Task) != task)
                {
                    // observe any later fault so it isn't reported as an unobserved task exception
                    _ = task.ContinueWith(t => _ = t.Exception, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                    throw new OperationCanceledException(cancelToken);
                }
            }

            return await task;
        }

        public static async Task<bool> TryWaitAsync(this SemaphoreSlim semaphore, CancellationToken cancelToken)
        {
            try
            {
                await semaphore.WaitAsync(cancelToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
