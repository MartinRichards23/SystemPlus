using System.Threading;

namespace SystemPlus.Threading
{
    public static class RetryHelper
    {
        /// <summary>
        /// Retries the operation until it fails and throws the exception
        /// </summary>
        /// <param name="maxAttempts">Max number of times to try</param>
        /// <param name="delay">Delay between each attempt</param>
        /// <param name="operation">The task to run</param>
        public static async Task<T> RetryOnException<T>(int maxAttempts, TimeSpan delay, bool backOff, Func<Task<T>> operation, CancellationToken cancelToken)
        {
            int attempts = 0;

            do
            {
                try
                {
                    cancelToken.ThrowIfCancellationRequested();

                    attempts++;
                    return await operation();
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch
                {
                    if (attempts >= maxAttempts)
                        throw;

                    // delay before next attempt
                    await Task.Delay(delay, cancelToken);

                    if (backOff)
                        delay *= 2;
                }
            } while (true);
        }

        /// <summary>
        /// Retries the operation until it fails and throws the exception
        /// </summary>
        /// <param name="maxAttempts">Max number of times to try</param>
        /// <param name="delay">Delay between each attempt</param>
        /// <param name="operation">The task to run</param>
        public static async Task RetryOnException(int maxAttempts, TimeSpan delay, bool backOff, Func<Task> operation, CancellationToken cancelToken)
        {
            int attempts = 0;

            do
            {
                try
                {
                    cancelToken.ThrowIfCancellationRequested();

                    attempts++;
                    await operation();
                    return;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch
                {
                    if (attempts >= maxAttempts)
                        throw;

                    // delay before next attempt
                    await Task.Delay(delay, cancelToken);

                    if (backOff)
                        delay *= 2;
                }
            } while (true);
        }
    }
}
