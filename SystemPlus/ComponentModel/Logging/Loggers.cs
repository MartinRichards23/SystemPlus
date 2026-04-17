using System.Diagnostics;
using System.IO;
using SystemPlus.IO;

namespace SystemPlus.ComponentModel.Logging
{
    /// <summary>
    /// Logs data to a file
    /// </summary>
    public class FileLogger : LoggerBase
    {
        readonly string filePath;
        readonly object key = new object();

        public FileLogger(string appName, string appVersion, string folder)
        {
            filePath = $"{folder}\\{appName} {appVersion} {DateTime.UtcNow:yyyy-MM-dd}.log";
        }

        public override void Write(MessageLevel level, string message, Exception error)
        {
            string text = string.Empty;

            if (error != null)
                text = error.ToString(true);

            if (message != null)
                text = message + "\r\n\r\n" + text;

            Write(level, text);
        }

        void Write(MessageLevel level, string text)
        {
            lock (key)
            {
                if (!File.Exists(filePath))
                {
                    string dir = Path.GetDirectoryName(filePath);

                    if (dir == null)
                        throw new NullReferenceException(nameof(dir));

                    FileSystem.EnsureDirExists(dir);
                }

                using (FileStream fs = new FileStream(filePath, FileMode.Append))
                using (StreamWriter w = new StreamWriter(fs))
                {
                    w.WriteLine("= {0}: {1} =", level, DateTime.UtcNow);
                    w.WriteLine(text);
                    w.WriteLine();
                }
            }
        }
    }

    /// <summary>
    /// Logs data to the debugger output
    /// </summary>
    public class OutputLogger : LoggerBase
    {
        public override void Write(MessageLevel level, string message, Exception error)
        {
            string text = string.Empty;

            if (error != null)
                text = error.ToString(true);

            if (message != null)
                text = message + "\r\n\r\n" + text;

            Debug.WriteLine(text);
        }
    }

    /// <summary>
    /// Logs data to the trace output
    /// </summary>
    public class TraceLogger : LoggerBase
    {
        public override void Write(MessageLevel level, string message, Exception error)
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
        public override void Write(MessageLevel level, string message, Exception error)
        {
            string text = string.Empty;

            if (error != null)
                text = error.ToString(true);

            if (message != null)
                text = message + "\r\n\r\n" + text;

            Console.WriteLine(text);
        }
    }
}
