﻿using System;
using System.Windows;

namespace SystemPlus.Windows
{
    public static class MessageBoxTools
    {
        public static MessageBoxResult InvokeShow(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return InvokeShow(messageBoxText, caption, button, icon, MessageBoxResult.OK);
        }

        public static MessageBoxResult InvokeShow(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
        {
            Func<MessageBoxResult> f = delegate
            {
                if (Application.Current?.MainWindow != null)
                    return MessageBox.Show(Application.Current.MainWindow, messageBoxText, caption, button, icon, defaultResult);

                return MessageBox.Show(messageBoxText, caption, button, icon, defaultResult);
            };

            if (Application.Current != null)
            {
                object o = Application.Current.Dispatcher.Invoke(f);
                return (MessageBoxResult)o;
            }
            return f();
        }
    }
}