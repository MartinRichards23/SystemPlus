using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace SystemPlus.Windows
{
    /// <summary>
    /// Exposes PInvoke methods
    /// </summary>
    public static class NativeMethods
    {
        #region User32 functions

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool flashWindowEx(ref FLASHWINFO pwfi);

        //[DllImport("user32.dll")]
        //public static extern IntPtr GetWindowDC(IntPtr hWnd);

        //[DllImport("user32.dll")]
        //public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hdc);

        //[DllImport("user32.dll")]
        //public static extern void ReleaseDC(IntPtr hdc);

        //[DllImport("user32.dll")]
        //public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool getCursorPos(ref Win32Point pt);

        [DllImport("user32.dll")]
        static extern bool setCursorPos(int x, int y);

        //[DllImport("user32.dll")]
        //public static extern int GetSystemMetrics(int index);

        //[DllImport("user32.dll")]
        //public static extern IntPtr GetDC(IntPtr windowHandle);

        //[DllImport("user32.dll", EntryPoint = "RedrawWindow")]
        //public static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, uint flags);

        //[DllImport("user32.dll")]
        //public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        //[DllImport("user32.dll", EntryPoint = "ShowWindow")]
        //static extern bool showWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        //[DllImport("user32.dll")]
        //public static extern bool ShowWindowAsync(int hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        static extern int setWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        static extern int getWindowLong(IntPtr hWnd, int nIndex);

        //[DllImport("user32.dll")]
        //public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        //[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        //public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        //[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        //public static extern int GetWindowTextLength(IntPtr hWnd);

        //public delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

        //public delegate bool WindowEnumDelegate(IntPtr hwnd, int lParam);

        //public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr lParam);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern bool EnumThreadWindows(int threadId, EnumThreadProc pfnEnum, IntPtr lParam);

        //[DllImport("user32.dll")]
        //public static extern int EnumChildWindows(IntPtr hwnd, WindowEnumDelegate del, int lParam);

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

        //[DllImport("user32.dll")]
        //public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out int lpdwProcessId);

        //[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        //public static extern bool SetWindowPos(int hWnd, // handle to window
        //    int hWndInsertAfter, // placement-order handle
        //    int x, // horizontal position
        //    int y, // vertical position
        //    int cx, // width
        //    int cy, // height
        //    uint uFlags // window-positioning options
        //    );

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool setWindowPos(IntPtr hWnd, // handle to window
            IntPtr hWndInsertAfter, // placement-order handle
            int x, // horizontal position
            int y, // vertical position
            int cx, // width
            int cy, // height
            uint uFlags // window-positioning options
            );

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr dc, DrawingOptions opts);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        //[DllImport("user32.dll")]
        //public static extern int SendMessage(int hWnd, int msg, int wParam, int lParam);

        //[DllImport("user32.dll")]
        //public static extern int SendMessage(IntPtr handle, int message, int wParam, int lParam);

        //[return: MarshalAs(UnmanagedType.Bool)]
        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        //[return: MarshalAs(UnmanagedType.Bool)]
        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern bool PostMessage(int hWnd, uint msg, int wParam, int lParam);

        //[return: MarshalAs(UnmanagedType.Bool)]
        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        //[DllImport("User32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdc, uint nFlags);

        //[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Unicode)]
        //public static extern int FindWindow(string strclassName, string strWindowName);

        //[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        //public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        //[DllImport("user32.dll")]
        //public static extern int SetForegroundWindow(int hWnd);

        //[DllImport("user32.dll")]
        //public static extern int SetForegroundWindow(IntPtr hWnd);

        //[DllImport("user32.dll", EntryPoint = "SetParent")]
        //public static extern int SetParent(int hWndChild, int hWndNewParent);

        //[DllImport("user32.dll", EntryPoint = "MoveWindow")]
        //public static extern bool MoveWindow(int hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        //[DllImport("user32.dll")]
        //public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        //[DllImport("user32.dll")]
        //public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        //[DllImport("gdi32.dll")]
        //public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        #endregion

        #region Gdi32 functions

        //[DllImport("gdi32.dll")]
        //public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        //[DllImport("gdi32.dll")]
        //public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        //[DllImport("gdi32.dll")]
        //public static extern bool DeleteDC(IntPtr hdc);

        //[DllImport("gdi32.dll")]
        //public static extern bool DeleteObject(IntPtr hObject);

        //[DllImport("gdi32.dll")]
        //public static extern IntPtr GetCurrentObject(IntPtr hdc, ushort objectType);

        //[DllImport("gdi32.dll")]
        //public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        #endregion

        #region Kernal32

        //[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        //public static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer, uint nSize, IntPtr arguments);

        //[DllImport("kernel32.dll")]
        //public static extern IntPtr LoadLibrary(string dllToLoad);

        //[DllImport("kernel32.dll")]
        //public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        //[DllImport("kernel32.dll")]
        //public static extern bool FreeLibrary(IntPtr hModule);

        //[DllImport("kernel32.dll")]
        //public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);

        #endregion

        public static Point GetCursorPos()
        {
            Win32Point w32Mouse = new Win32Point();
            getCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public static void SetCursorPos(Point point)
        {
            setCursorPos((int)point.X, (int)point.Y);
        }

        public static bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow)
        {
            return ShowWindow(hWnd, nCmdShow);
        }

        public static int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong)
        {
            return setWindowLong(hWnd, nIndex, dwNewLong);
        }

        public static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            return getWindowLong(hWnd, nIndex);
        }

        public static bool SetWindowPos(IntPtr hWnd, // handle to window
            IntPtr hWndInsertAfter, // placement-order handle
            int x, // horizontal position
            int y, // vertical position
            int cx, // width
            int cy, // height
            uint uFlags // window-positioning options
            )
        {
            return setWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Win32Point
        {
            public int X;
            public int Y;
        };

        //[Flags]
        //public enum DrawingOptions
        //{
        //    PRF_CHECKVISIBLE = 0x00000001,
        //    PRF_NONCLIENT = 0x00000002,
        //    PRF_CLIENT = 0x00000004,
        //    PRF_ERASEBKGND = 0x00000008,
        //    PRF_CHILDREN = 0x00000010,
        //    PRF_OWNED = 0x00000020
        //}

        //public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

        #region Ini file stuff

        //[DllImport("kernel32", CharSet = CharSet.Unicode)]
        //static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        //[DllImport("kernel32", CharSet = CharSet.Unicode)]
        //static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        //static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);

        //public static void WriteIniValue(string section, string key, string value, string path)
        //{
        //    try
        //    {
        //        WritePrivateProfileString(section, key, value, path);
        //    }
        //    catch
        //    {
        //    }
        //}

        //public static string ReadIniString(string section, string key, string path)
        //{
        //    StringBuilder temp = new StringBuilder(255);
        //    int i = GetPrivateProfileString(section, key, "", temp, 255, path);

        //    if (i == 0)
        //        return string.Empty;

        //    return temp.ToString().Trim();
        //}

        //public static bool GetPrivateProfileSection(string sectionName, string fileName, out string[]? section)
        //{
        //    section = null;

        //    if (!File.Exists(fileName))
        //        return false;

        //    const uint maxBuffer = 32767;

        //    IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)maxBuffer);

        //    uint bytesReturned = GetPrivateProfileSection(sectionName, pReturnedString, maxBuffer, fileName);

        //    if ((bytesReturned == maxBuffer - 2) || (bytesReturned == 0))
        //    {
        //        Marshal.FreeCoTaskMem(pReturnedString);
        //        return false;
        //    }

        //    // NOTE: Calling Marshal.PtrToStringAuto(pReturnedString) will
        //    // result in only the first pair being returned
        //    string? returnedString = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned - 1);

        //    section = returnedString?.Split('\0');

        //    Marshal.FreeCoTaskMem(pReturnedString);
        //    return true;
        //}

        #endregion

        #region FlashWindow

        [StructLayout(LayoutKind.Sequential)]
        struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        enum Flashenum : uint
        {
            //Stop flashing. The system restores the window to its original state. 
            FLASHW_STOP = 0,

            //Flash the window caption. 
            FLASHW_CAPTION = 1,

            //Flash the taskbar button. 
            FLASHW_TRAY = 2,

            //Flash both the window caption and taskbar button.
            //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
            FLASHW_ALL = 3,

            //Flash continuously, until the FLASHW_STOP flag is set. 
            FLASHW_TIMER = 4,

            //Flash continuously until the window comes to the foreground. 
            FLASHW_TIMERNOFG = 12
        }

        public static bool FlashWindow(IntPtr hWnd)
        {
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = (uint)Flashenum.FLASHW_ALL | (uint)Flashenum.FLASHW_TIMERNOFG;
            fInfo.uCount = uint.MaxValue;
            fInfo.dwTimeout = 0;

            return flashWindowEx(ref fInfo);
        }

        #endregion

        //public static int MakeLParam(int loWord, int hiWord)
        //{
        //    return ((hiWord << 16) | (loWord & 0xffff));
        //}

        //[Flags]
        //public enum MouseEventFlags
        //{
        //    LEFTDOWN = 0x00000002,
        //    LEFTUP = 0x00000004,
        //    MIDDLEDOWN = 0x00000020,
        //    MIDDLEUP = 0x00000040,
        //    MOVE = 0x00000001,
        //    ABSOLUTE = 0x00008000,
        //    RIGHTDOWN = 0x00000008,
        //    RIGHTUP = 0x00000010
        //}

        //public enum MouseMessages
        //{
        //    WM_LBUTTONDOWN = 0x0201,
        //    WM_LBUTTONUP = 0x0202,
        //    WM_MOUSEMOVE = 0x0200,
        //    WM_MOUSEWHEEL = 0x020A,
        //    WM_RBUTTONDOWN = 0x0204,
        //    WM_RBUTTONUP = 0x0205,
        //    WM_MOUSEACTIVATE = 0x21
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //public struct RECT
        //{
        //    public int left;
        //    public int top;
        //    public int right;
        //    public int bottom;

        //    public RECT(int left, int top, int right, int bottom)
        //    {
        //        this.left = left;
        //        this.top = top;
        //        this.right = right;
        //        this.bottom = bottom;
        //    }
        //}

        //public enum WindowPlacement
        //{
        //    HWD_NOTOPMOST = -2,
        //    HWD_TOPMOST = -1,
        //    HWD_TOP = 0,
        //    HWD_BOTTOM = 1
        //}

        //[Flags]
        //public enum SendMessageFlags
        //{
        //    HWND_TOP = 0x0,
        //    WM_COMMAND = 0x0112,
        //    WM_QT_PAINT = 0xC2DC,
        //    WM_PAINT = 0x000F,
        //    WM_SIZE = 0x0005,
        //    SWP_FRAMECHANGED = 0x0020
        //}

        public enum WindowShowStyle : uint
        {
            Hide = 0,
            ShowNormal = 1,
            ShowMinimized = 2,
            ShowMaximized = 3,
            ShowNormalNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActivate = 7,
            ShowNoActivate = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimized = 11
        }

        //[Flags]
        //public enum WindowPosition : int
        //{
        //    SWP_NOSIZE = 0x0001,
        //    SWP_NOMOVE = 0x0002,
        //    SWP_NOZORDER = 0x0004,
        //    SWP_NOREDRAW = 0x0008,
        //    SWP_NOACTIVATE = 0x0010,
        //    SWP_FRAMECHANGED = 0x0020,
        //    SWP_SHOWWINDOW = 0x0040,
        //    SWP_HIDEWINDOW = 0x0080,
        //    SWP_NOCOPYBITS = 0x0100,
        //    SWP_NOOWNERZORDER = 0x0200,
        //    SWP_NOSENDCHANGING = 0x0400
        //}

        //public const int WM_PAINT = 0xF;
        //public const int WM_PRINT = 0x0317;
        //public const int WM_PRINTCLIENT = 0x0318;
        //public const int WM_SIZE = 0x0005;
        public const int SWP_FRAMECHANGED = 0x0020;
        //public const int WM_QUIT = 0x0012;
        //public const int HWND_TOP = 0x0;
        //public const int WM_COMMAND = 0x0112;
        //public const int WM_QT_PAINT = 0xC2DC;

        //public const int WM_KEYDOWN = 0x0100;
        //public const int WM_SYSKEYDOWN = 0x0104;
        //public const int WM_KEYUP = 0x0101;

        public const int GWL_EXSTYLE = -20;
        public const int GWL_STYLE = -16;
        //public const int WS_EX_LAYERED = 0x80000;
        //public const int LWA_ALPHA = 0x2;
        //public const int WS_EX_TOOLWINDOW = 0x80;
        //public const int WS_EX_APPWINDOW = 0x40000;
        public const int WS_SYSMENU = 0x80000;
        public const int WS_EX_DLGMODALFRAME = 0x0001;

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int WS_MAXIMIZEBOX = 0x00010000;
        public const int WS_MINIMIZEBOX = 0x00020000;

    }
}