using System;
using System.Net;
using System.Windows;
using SystemPlus.Reflection;

namespace SystemPlus.Windows
{
    /// <summary>
    /// Shows error messageboxes
    /// </summary>
    public class ErrorMessageBox
    {
        public static void Show(Exception error)
        {
            if (error is OperationCanceledException)
                return;

            if (error is AggregateException aggEx)
            {
                if (aggEx.InnerException != null)
                    Show(aggEx.InnerException);

                return;
            }

            string text;

            // Make text depending on what kind of error it is
            if (error is WebException webEx)
            {
                if (webEx.Response is HttpWebResponse response)
                    text = string.Format("An internet problem has occurred, code: {0}", response.StatusCode);
                else
                    text = string.Format("An internet problem has occurred, code: {0}", webEx.Status);
            }
            else if (error is ArgumentNullException || error is NullReferenceException || error is ArgumentOutOfRangeException || error is IndexOutOfRangeException)
            {
                text = "A problem has occurred, the error has been logged.";
            }
            else
            {
                text = "A problem has occurred:\r\n\r\n" + error.Message;
            }

            text = text.Trim();

            Show(text, ReflectionExtensions.ProductName ?? string.Empty);
        }

        public static void Show(string message)
        {
            Show(message, ReflectionExtensions.ProductName ?? string.Empty);
        }

        protected static void Show(string text, string title)
        {
            MessageBoxTools.InvokeShow(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}