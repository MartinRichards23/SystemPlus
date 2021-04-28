using System;

namespace SystemPlus.ComponentModel.Logging
{
    public interface ILogger
    {
        void Write(MessageLevel level, string? message = null, Exception? error = null);
        void Flush();

        MessageLevel MinLevel { get; }
    }
}