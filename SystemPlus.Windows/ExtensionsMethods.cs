using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace SystemPlus.Windows
{
    public static class ExtensionsMethods
    {
        public static T FindResource<T>(this FrameworkElement element, object resourceKey)
        {
            return (T)element.FindResource(resourceKey);
        }

        public static T FindResource<T>(this Application app, object resourceKey)
        {
            return (T)app.FindResource(resourceKey);
        }

        public static T? GetItemByName<T>(this ItemCollection items, string name) where T : FrameworkElement
        {
            foreach (T fe in items.OfType<T>())
            {
                if (fe.Name == name)
                    return fe;
            }

            return null;
        }

        /// <summary>
        /// Multiply the width and height of a Size
        /// </summary>
        public static Size Multiply(this Size size, double value)
        {
            size.Width *= value;
            size.Height *= value;
            return size;
        }

        /// <summary>
        /// Multiply the X and Y of a Point
        /// </summary>
        public static Point Multiply(this Point point, double value)
        {
            point.X *= value;
            point.Y *= value;
            return point;
        }

        /// <summary>
        /// Centres the window on the given point
        /// </summary>
        public static void CentreOnPoint(this Window window, Point point)
        {
            window.Left = (int)(point.X - (window.ActualWidth / 2));
            window.Top = (int)(point.Y - (window.ActualHeight / 2));
        }

        /// <summary>
        /// Ensure the window is not off the bounds of the screen
        /// </summary>
        public static void EnsureOnScreen(this Window window)
        {
            double screenH = SystemParameters.VirtualScreenHeight;
            double screenW = SystemParameters.VirtualScreenWidth;

            double maxX = window.Left + window.ActualWidth;
            double maxY = window.Top + window.ActualHeight;

            if (maxX > screenW)
                window.Left -= maxX - screenW;
            if (maxY > screenH)
                window.Top -= maxY - screenH;
            if (window.Left < 0)
                window.Left = 0;
            if (window.Top < 0)
                window.Top = 0;
        }

        /// <summary>
        /// Opens the popup for a specified duration
        /// </summary>
        public static void Open(this Popup popup, TimeSpan duration)
        {
            popup.IsOpen = true;

            DispatcherTimer time = new DispatcherTimer
            {
                Interval = duration
            };
            time.Start();
            time.Tick += delegate
            {
                time.Stop();
                popup.IsOpen = false;
            };
        }

        /// <summary>
        /// Clears all formatting from the text
        /// </summary>
        public static void ClearAllFormatting(this RichTextBox rtb)
        {
            TextRange txtRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            txtRange.ClearAllProperties();
        }

        /// <summary>
        /// Gets the bounding rectangle of a collection of points
        /// </summary>
        public static Rect GetBoundingRect(this IEnumerable<Point> points)
        {
            double maxX = double.MinValue;
            double minX = double.MaxValue;
            double maxY = double.MinValue;
            double minY = double.MaxValue;

            foreach (Point p in points)
            {
                maxX = Math.Max(maxX, p.X);
                minX = Math.Min(minX, p.X);
                maxY = Math.Max(maxY, p.Y);
                minY = Math.Min(minY, p.Y);
            }

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Gets the centre of the rectangle
        /// </summary>
        public static Point Center(this Rect rect)
        {
            return new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
        }

        public static Task<bool?> ShowDialogAsync(this Window window)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            TaskCompletionSource<bool?> completion = new TaskCompletionSource<bool?>();
            window.Dispatcher.BeginInvoke(new Action(() => completion.SetResult(window.ShowDialog())));

            return completion.Task;
        }

        /// <summary>
        /// Gets the main application window, using Invoke()
        /// </summary>
        public static Window InvokeGetMainWindow(this Application application)
        {
            if (!application.Dispatcher.CheckAccess())
            {
                object res = application.Dispatcher.Invoke(new Func<Application, Window>(InvokeGetMainWindow), application);
                return (Window)res;
            }

            return application.MainWindow;
        }

        /// <summary>
        /// Gets the main active application window
        /// </summary>
        public static Window? GetActiveWindow(this Application application)
        {
            return application.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
        }

        /// <summary>
        /// Equivalent of Application.DoEvents()
        /// </summary>
        public static void DoEvents(this Application application)
        {
            application.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }

        #region Random extensions

        public static Color NextColor(this Random random, byte minValue = 0)
        {
            return Color.FromArgb(255, (byte)random.Next(minValue, 255), (byte)random.Next(minValue, 255), (byte)random.Next(minValue, 255));
        }

        /// <summary>
        /// Gets a random point, within a maximum deviation of an origin
        /// </summary>
        public static Point NextPoint(this Random random, Point origin, double maxDeviation)
        {
            double x = (random.NextDouble() * 2) - 1;
            double y = (random.NextDouble() * 2) - 1;

            x *= maxDeviation;
            y *= maxDeviation;

            return new Point(origin.X + x, origin.Y + y);
        }

        #endregion

        #region Dispatcher

        /// <summary>
        /// Invokes action, throwing excetions on current thread if they occur
        /// </summary>
        public static void InvokeWithExceptions(this Dispatcher dispatcher, Action action)
        {
            Exception? error = null;
            Action wrapper = () =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            };

            dispatcher.Invoke(DispatcherPriority.Send, wrapper);

            // if error occured then throw it
            if (error != null)
                throw (error);
        }

        /// <summary>
        /// Executes the delegate asyncronously if called from seperate thread, else calls normally
        /// </summary>
        public static void BeginInvokeIfNeeded(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
        {
            if (!dispatcher.CheckAccess())
                dispatcher.BeginInvoke(action, priority);
            else
                action();
        }

        /// <summary>
        /// Executes the delegate asyncronously if called from seperate thread, else calls normally
        /// </summary>
        public static void BeginInvokeIfNeeded(this Dispatcher dispatcher, Action action)
        {
            dispatcher.BeginInvokeIfNeeded(action, DispatcherPriority.Normal);
        }

        /// <summary>
        /// Invokes the action after a delay
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public static void DelayedInvoke(this Dispatcher dispatcher, Action action, TimeSpan delay)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal, dispatcher);

            void handler(object? sender, EventArgs e)
            {
                // Stop the timer so it won't keep executing every X seconds
                // and also avoid keeping the handler in memory.
                dispatcherTimer.Tick -= handler;
                dispatcherTimer.Stop();

                // Perform the action.
                dispatcher.Invoke(action);
            }

            dispatcherTimer.Tick += handler;
            dispatcherTimer.Interval = delay;
            dispatcherTimer.Start();
        }

        #endregion
    }
}