using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using SystemPlus.IO;
using SystemPlus.Reflection;
using SystemPlus.Text;

namespace SystemPlus.ComponentModel.Logging
{
    /// <summary>
    /// Logs data to a file
    /// </summary>
    public class FileLogger : LoggerBase
    {
        readonly string filePath;
        readonly object key = new object();

        public FileLogger(string folder)
        {
            string? name;
            Version? version;

            try
            {
                name = ReflectionExtensions.ProductName;
                version = ReflectionExtensions.ProductVersion;
            }
            catch
            {
                name = ReflectionExtensions.CallerName;
                version = ReflectionExtensions.CallerVersion;
            }

            filePath = $"{folder}\\{name} {version} {DateTime.UtcNow:yyyy-MM-dd}.log";
        }

        public FileLogger(string appName, string appVersion, string folder)
        {
            filePath = $"{folder}\\{appName} {appVersion} {DateTime.UtcNow:yyyy-MM-dd}.log";
        }

        public override void WriteInfo(string message)
        {
            Write(MessageLevel.Info, message);
        }

        public override void WriteWarning(string message)
        {
            Write(MessageLevel.Warning, message);
        }

        public override void WriteError(string? message, Exception? error)
        {
            string text = string.Empty;

            if (error != null)
                text = error.ToString(true);

            if (message != null)
                text = message + "\r\n\r\n" + text;

            Write(MessageLevel.Error, text);
        }

        void Write(MessageLevel level, string text)
        {
            lock (key)
            {
                bool writeHeader = false;
                if (!File.Exists(filePath))
                {
                    string? dir = Path.GetDirectoryName(filePath);

                    if (dir == null)
                        throw new NullReferenceException(nameof(dir));

                    FileSystem.EnsureDirExists(dir);

                    writeHeader = true;
                }

                using FileStream fs = new FileStream(filePath, FileMode.Append);
                using StreamWriter w = new StreamWriter(fs);

                if (writeHeader)
                {
                    string header = MakeLogHeader();
                    w.WriteLine(header);
                    w.WriteLine();
                }

                w.WriteLine("===== {0}: {1} =====", level, DateTime.UtcNow);
                w.WriteLine(text);
                w.WriteLine();
            }
        }
    }

    /// <summary>
    /// Logs data to the trace output
    /// </summary>
    public class TraceLogger : LoggerBase
    {
        public override void WriteInfo(string message)
        {
            Trace.TraceInformation(message);
        }

        public override void WriteWarning(string message)
        {
            Trace.TraceWarning(message);
        }

        public override void WriteError(string? message, Exception? error)
        {
            string text = string.Empty;

            if (error != null)
                text = error.ToString(true);

            if (message != null)
                text = message + "\r\n\r\n" + text;

            Trace.TraceError(text);
        }
    }

    /// <summary>
    /// Logs data to the console
    /// </summary>
    public class ConsoleLogger : LoggerBase
    {
        public override void WriteInfo(string message)
        {
            Console.WriteLine(message);
        }

        public override void WriteWarning(string message)
        {
            Console.WriteLine(message);
        }

        public override void WriteError(string? message, Exception? error)
        {
            string text = string.Empty;

            if (error != null)
                text = error.ToString(true);

            if (message != null)
                text = message + "\r\n\r\n" + text;

            Console.WriteLine(text);
        }
    }

    /// <summary>
    /// Logs data to email
    /// </summary>
    public class EmailLogger : LoggerBase
    {
        readonly StringBuilder buffer = new StringBuilder();
        readonly object key = new object();

        readonly IList<string> addresses = new List<string>();

        readonly string host;
        readonly string from;
        readonly string password;
        DateTime lastFlush = DateTime.MinValue;

        public EmailLogger(MessageLevel minLevel, string host, string password, string from, string to)
        {
            MinLevel = minLevel;
            this.host = host;
            this.password = password;
            this.from = from;

            addresses.Add(to);
        }

        public override void WriteInfo(string message)
        {
            BufferMessage(MessageLevel.Info, message);
        }

        public override void WriteWarning(string message)
        {
            BufferMessage(MessageLevel.Warning, message);
        }

        public override void WriteError(string? message, Exception? error)
        {
            string text = string.Empty;

            if (error != null)
                text = error.ToString(true);

            if (message != null)
                text = message + "\r\n\r\n" + text;

            BufferMessage(MessageLevel.Error, text);
        }

        void BufferMessage(MessageLevel level, string text)
        {
            lock (key)
            {
                // ignore empty messages
                if (string.IsNullOrWhiteSpace(text))
                    return;

                // don't write too many chars to the buffer
                if (buffer.Length >= 10000)
                    return;

                buffer.AppendLine("===== {0}: {1} =====", level, DateTime.UtcNow);
                buffer.AppendLine(text);
                buffer.AppendLine();

                // Flush if haven't done so for 6 hours
                TimeSpan ts = DateTime.UtcNow - lastFlush;
                if (ts.TotalHours > 6)
                    Flush();
            }
        }

        public override void Flush()
        {
            string subject = "Error log " + DateTime.UtcNow.ToStringStandard();
            string body;

            lock (key)
            {
                if (buffer.Length < 1)
                    return;

                string header = MakeLogHeader();

                // see if there is data to send
                body = header + "\r\n\r\n" + buffer;
                buffer.Clear();
            }

            lastFlush = DateTime.UtcNow;

            using MailMessage message = new MailMessage();
            using SmtpClient smtp = new SmtpClient(host);

            foreach (string address in addresses)
            {
                message.To.Add(address);
            }

            message.From = new MailAddress(from);
            message.Subject = subject;
            message.Body = body;
            smtp.Credentials = new NetworkCredential(from, password);
            smtp.Send(message);
        }
    }
}