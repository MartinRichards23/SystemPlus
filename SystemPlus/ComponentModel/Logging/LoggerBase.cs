﻿using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SystemPlus.ComponentModel.Logging
{
    /// <summary>
    /// Base class for loggers
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        public MessageLevel MinLevel { get; set; } = MessageLevel.Info;

        public abstract void WriteInfo(string message);
        public abstract void WriteWarning(string message);
        public abstract void WriteError(string message, Exception ex);

        public virtual void Flush()
        {
        }

        protected string WriteLogHeader()
        {
            StringBuilder writer = new StringBuilder();


            try
            {
                AssemblyName assem = Assembly.GetEntryAssembly().GetName();
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