using System.Diagnostics;

namespace SystemPlus.Diagnostics
{
    public static class DebugTimer
    {
        static readonly Stopwatch stopwatch = new Stopwatch();

        [Conditional("DEBUG")]
        public static void Start(string text)
        {
            stopwatch.Restart();

            if (!string.IsNullOrEmpty(text))
            {
                Debug.WriteLine(text);
            }
        }

        [Conditional("DEBUG")]
        public static void Start()
        {
            Start(null);
        }

        [Conditional("DEBUG")]
        public static void Reset(string text)
        {
            if (!string.IsNullOrEmpty(text))
                Message(text);

            stopwatch.Restart();
        }

        [Conditional("DEBUG")]
        public static void Message(string text)
        {
            double time = stopwatch.Elapsed.TotalMilliseconds;
            Debug.WriteLine("{0}: {1} ms", text, time);
        }

        [Conditional("DEBUG")]
        public static void Stop()
        {
            Stop("===== Timer stopped =====");
        }

        [Conditional("DEBUG")]
        public static void Stop(string message)
        {
            if (!string.IsNullOrEmpty(message))
                Message(message);

            stopwatch.Stop();
        }
    }
}