using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SystemPlus.Web.Logging
{
    public static class LoggingExtensions
    {
        public static Task ContinueWithLogErrors(this Task task, ILogger logger, [CallerMemberName] string message = "")
        {
            return task.ContinueWith(t =>
            {
                if (t.Exception != null)
                    logger.LogError(t.Exception, message);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
