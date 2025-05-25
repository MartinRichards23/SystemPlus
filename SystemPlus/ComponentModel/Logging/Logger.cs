using System.Globalization;
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

        public event Action<MessageLevel, string?, Exception?>? MessageLogged;

        Exception? lastError;
        DateTime lastErrorTime;
        readonly TimeSpan minErrorTime = TimeSpan.FromSeconds(30);

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

        /// <summary>
        /// Record information
        /// </summary>
        public void LogInfo(string message, params object[] args)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (!args.IsNullOrEmpty())
                message = string.Format(CultureInfo.InvariantCulture, message, args);

            Log(MessageLevel.Info, message, null);
        }

        /// <summary>
        /// Record a warning
        /// </summary>
        public void LogWarning(string message, params object[] args)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (!args.IsNullOrEmpty())
                message = string.Format(CultureInfo.InvariantCulture, message, args);


            Log(MessageLevel.Warning, message, null);
        }

        /// <summary>
        /// Record an error
        /// </summary>
        public void LogError(Exception? error)
        {
            LogError(null, error);
        }

        /// <summary>
        /// Record an error message
        /// </summary>
        public void LogError(string? message)
        {
            LogError(message, null);
        }

        /// <summary>
        /// Record an error
        /// </summary>
        public void LogError(string? message, Exception? error)
        {
            Log(MessageLevel.Error, message, error);
        }

        public void Log(MessageLevel level, string? message, Exception? error)
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
                    if (lb.MinLevel > level)
                        continue;

                    lb.Write(level, message, error);
                }
                catch
                {
                }
            }

            OnMessageLogged(level, message, error);
        }


        protected void OnMessageLogged(MessageLevel level, string? message, Exception? error)
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

        public static Logger Default { get; } = new Logger();

        #endregion
    }

    public enum MessageLevel
    {
        Info,
        Warning,
        Error
    }
}