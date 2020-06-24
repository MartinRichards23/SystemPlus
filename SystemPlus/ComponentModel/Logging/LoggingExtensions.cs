using System.Threading.Tasks;

namespace SystemPlus.ComponentModel.Logging
{
    public static class LoggingExtensions
    {
        public static Task ContinueWithLogErrors(this Task task)
        {
            return task.ContinueWith(t =>
            {
                if (t.Exception != null)
                    Logger.Default.LogError(t.Exception);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
