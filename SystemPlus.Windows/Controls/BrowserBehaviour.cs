﻿using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace SystemPlus.Windows.Controls
{
    public class BrowserBehavior
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached("Html", typeof(string), typeof(BrowserBehavior), new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser d)
        {
            return (string)d.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser d, string value)
        {
            d.SetValue(HtmlProperty, value);
        }

        static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser webBrowser = dependencyObject as WebBrowser;
            string html = e.NewValue as string;

            if (webBrowser != null && html != null)
                webBrowser.NavigateToString(html);
        }

        public static void HideScriptErrors(WebBrowser wb, bool hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);

            if (fiComWebBrowser == null)
                return;

            object objComWebBrowser = fiComWebBrowser.GetValue(wb);

            if (objComWebBrowser == null)
                return;

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }
    }
}