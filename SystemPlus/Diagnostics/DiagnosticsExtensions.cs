using System;
using System.Diagnostics;

namespace SystemPlus.Diagnostics
{
    /// <summary>
    /// Extensions and utilities for diagnostics activities
    /// </summary>
    public static class DiagnosticsExtensions
    {
        /// <summary>
        /// Is running in debug mode
        /// </summary>
        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Returns value indicating if this application is already open
        /// </summary>
        public static bool IsApplicationOpen()
        {
            try
            {
                Process thisProcess = Process.GetCurrentProcess();
                Process[] procList = Process.GetProcessesByName(thisProcess.ProcessName);

                if (procList.Length > 1)
                    return true;
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Closes all other instances
        /// </summary>
        public static void CloseOtherInstances()
        {
            Process thisProcess = Process.GetCurrentProcess();
            Process[] procList = Process.GetProcessesByName(thisProcess.ProcessName);

            if (procList.Length == 1)
                return; // There's just the current process.

            for (uint i = 0; i < procList.Length; i++)
            {
                // check start time, as mainwindow handle is zero if its hidden and the other handles vary.
                if (procList[i].StartTime == thisProcess.StartTime)
                    continue;

                try
                {
                    procList[i].Kill();
                    procList[i].WaitForExit(10000);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Closes all other instances that have a hidden main window
        /// </summary>
        public static void CloseOtherHiddenInstances()
        {
            Process thisProcess = Process.GetCurrentProcess();
            Process[] procList = Process.GetProcessesByName(thisProcess.ProcessName);

            if (procList.Length == 1)
                return; // There's just the current process.

            for (uint i = 0; i < procList.Length; i++)
            {
                // check start time, as mainwindow handle is zero if its hidden and the other handles vary.
                if (procList[i].StartTime == thisProcess.StartTime)
                    continue;

                // is it hidden?
                if (procList[i].MainWindowHandle != IntPtr.Zero)
                    continue;

                try
                {
                    procList[i].Kill();
                    procList[i].WaitForExit(10000);
                }
                catch
                {
                }
            }
        }
    }
}