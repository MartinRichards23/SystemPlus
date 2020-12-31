using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public static async Task<T> RetryOnException<T>(int maxAttempts, TimeSpan delay, Func<Task<T>> operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            int attempts = 0;

            do
            {
                try
                {
                    attempts++;
                    return await operation();
                }
                catch
                {
                    if (attempts >= maxAttempts)
                        throw;

                    // delay before next attempt
                    await Task.Delay(delay);
                }
            } while (true);
        }

        /// <summary>
        /// Retries the operation until it fails and throws the exception
        /// </summary>
        /// <param name="maxAttempts">Max number of times to try</param>
        /// <param name="delay">Delay between each attempt</param>
        /// <param name="operation">The task to run</param>
        public static async Task RetryOnException(int maxAttempts, TimeSpan delay, Func<Task> operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            int attempts = 0;

            do
            {
                try
                {
                    attempts++;
                    await operation();
                    return;
                }
                catch
                {
                    if (attempts >= maxAttempts)
                        throw;

                    // delay before next attempt
                    await Task.Delay(delay);
                }
            } while (true);
        }
    }
}
