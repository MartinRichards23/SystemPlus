using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemPlus.Collections.Generic;

namespace SystemPlus.ComponentModel.Logging
{
    /// <summary>
    /// Class for handling and recording exceptions
    /// </summary>
    public class Logger
    {
        #region Fields

        readonly IList<ILogger> loggers = new List<ILogger>();

        static readonly Logger defaultLog = new Logger();

        public event Action<MessageLevel, string, Exception> MessageLogged;

        Exception lastError;
        DateTime lastErrorTime;
        TimeSpan minErrorTime = TimeSpan.FromSeconds(30);

        #endregion

        /// <summary>
        /// Active loggers
        /// </summary>
        public IEnumerable<ILogger> Loggers
        {
            get { return loggers; }
        }

        #region Methods

        public void AddLogger(ILogger logger)
        {
            loggers.Add(logger);
        }

        public void LogInfoAsync(string message, params object[] args)
        {
            Task.Factory.StartNew(() =>
            {
                LogInfo(message, args);
            });
        }

        /// <summary>
        /// Record information
        /// </summary>
        public void LogInfo(string message, params object[] args)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (!args.IsNullOrEmpty())
                message = string.Format(message, args);

            foreach (ILogger lb in loggers)
            {
                try
                {
                    if (lb.MinLevel > MessageLevel.Info)
                        continue;

                    lb.WriteInfo(message);
                }
                catch
                {
                }
            }

            OnMessageLogged(MessageLevel.Info, message, null);
        }

        public void LogWarningAsync(string message, params object[] args)
        {
            Task.Factory.StartNew(() =>
            {
                LogWarning(message, args);
            });
        }

        /// <summary>
        /// Record a warning
        /// </summary>
        public void LogWarning(string message, params object[] args)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (!args.IsNullOrEmpty())
                message = string.Format(message, args);

            foreach (ILogger lb in loggers)
            {
                try
                {
                    if (lb.MinLevel > MessageLevel.Warning)
                        continue;

                    lb.WriteWarning(message);
                }
                catch
                {
                }
            }

            OnMessageLogged(MessageLevel.Warning, message, null);
        }

        /// <summary>
        /// Record an error
        /// </summary>
        public void LogError(Exception error)
        {
            LogError(null, error);
        }

        /// <summary>
        /// Record an error message
        /// </summary>
        public void LogError(string message)
        {
            LogError(message, null);
        }

        /// <summary>
        /// Record an error
        /// </summary>
        public void LogError(string message, Exception error)
        {
            if (error != null)
            {
                // ignore operation cancelled errors
                if (error is OperationCanceledException)
                    return;

                // ignore minor exceptions
                if (error is MinorException)
                    return;

                // Recurse AggregateExceptions
                if (error is AggregateException agg)
                {
                    if (agg.InnerExceptions != null)
                    {
                        foreach (Exception inner in agg.InnerExceptions)
                        {
                            LogError(message, inner);
                        }
                    }

                    return;
                }

                DateTime now = DateTime.UtcNow;

                // don't spam lots of same errors in short time
                if (lastError != null && lastError.Message == error.Message)
                {
                    if (now - lastErrorTime < minErrorTime)
                    {
                        // not enough time since last identical error
                        return;
                    }
                }

                lastError = error;
                lastErrorTime = now;
            }

            // Call all of our loggers
            foreach (ILogger lb in loggers)
            {
                try
                {
                    if (lb.MinLevel > MessageLevel.Error)
                        continue;

                    lb.WriteError(message, error);
                }
                catch
                {
                }
            }

            OnMessageLogged(MessageLevel.Error, message, error);
        }

        protected void OnMessageLogged(MessageLevel level, string message, Exception error)
        {
            try
            {
                MessageLogged?.Invoke(level, message, error);
            }
            catch
            {
            }
        }

        public void Flush()
        {
            foreach (ILogger lb in loggers)
            {
                try
                {
                    lb.Flush();
                }
                catch
                {
                }
            }
        }

        #endregion

        #region Statics

        public static Logger Default
        {
            get { return defaultLog; }
        }

        #endregion
    }

    public enum MessageLevel
    {
        Info,
        Warning,
        Error
    }
}