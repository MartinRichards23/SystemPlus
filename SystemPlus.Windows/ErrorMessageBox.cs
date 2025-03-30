using System;
using System.Net;
using System.Windows;

namespace SystemPlus.Windows
{
    /// <summary>
    /// Shows error messageboxes
    /// </summary>
    public static class ErrorMessageBox
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
                    text = $"An internet problem has occurred, code: {response.StatusCode}";
                else
                    text = $"An internet problem has occurred, code: {webEx.Status}";
            }
            else if (error is ArgumentNullException || error is NullReferenceException || error is ArgumentOutOfRangeException || error is IndexOutOfRangeException)
            {
                text = "A problem has occurred, the error has been logged.";
            }
            else
            {
                text = "A problem has occurred:\r\n\r\n" + error?.Message;
            }

            text = text.Trim();

            Show(text, string.Empty);
        }

        public static void Show(string message)
        {
            Show(message, string.Empty);
        }

        static void Show(string text, string title)
        {
            MessageBoxTools.InvokeShow(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}