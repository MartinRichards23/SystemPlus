using System.Globalization;
using System.Threading;

namespace SystemPlus.ComponentModel.Logging
{
    /// <summary>
    /// Base class for loggers
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        public MessageLevel MinLevel { get; set; } = MessageLevel.Info;

        public abstract void Write(MessageLevel level, string? message, Exception? error);

        public virtual void Flush()
        {
        }

        protected static string MakeLogHeader()
        {
            StringBuilder writer = new StringBuilder();

            try
            {
                CultureInfo culture = Thread.CurrentThread.CurrentCulture;

                writer.AppendLine("Utc time: " + DateTime.UtcNow);
                writer.AppendLine("Cpus: " + Environment.ProcessorCount.ToString(CultureInfo.InvariantCulture));
                writer.AppendLine("OS version: " + Environment.OSVersion);
                writer.AppendLine("Culture: " + culture?.TwoLetterISOLanguageName);
                writer.AppendLine(".NET version: " + Environment.Version);
            }
            catch
            { }

            return writer.ToString();
        }
    }
}