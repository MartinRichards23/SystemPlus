using System;

namespace SystemPlus.ComponentModel.Logging
{
    public interface ILogger
    {
        void WriteError(string message, Exception ex);
        void WriteInfo(string message);
        void WriteWarning(string message);
        void Flush();

        MessageLevel MinLevel { get; }
    }
}