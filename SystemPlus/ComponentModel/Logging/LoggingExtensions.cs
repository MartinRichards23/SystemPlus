using System.Threading;

namespace SystemPlus.ComponentModel.Logging
{
    public static class LoggingExtensions
    {
        public static Task ContinueWithLogErrors(this Task task)
        {
            if (task == null)
                return Task.CompletedTask;

            return task.ContinueWith(t =>
            {
                if (t.Exception != null)
                    Logger.Default.LogError(t.Exception);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
        }
    }
}
