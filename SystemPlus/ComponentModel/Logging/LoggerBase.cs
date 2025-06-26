using System;

namespace SystemPlus.ComponentModel.Logging
{
    /// <summary>
    /// Base class for loggers
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        public MessageLevel MinLevel { get; set; } = MessageLevel.Info;

        public abstract void Write(MessageLevel level, string message, Exception error);

        public virtual void Flush()
        {
        }
    }
}
