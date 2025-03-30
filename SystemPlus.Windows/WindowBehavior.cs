using System.Windows;
using System.Windows.Interop;

namespace SystemPlus.Windows
{
    public static class WindowBehavior
    {
        static readonly Type ownerType = typeof(WindowBehavior);

        #region HideCloseButton (attached property)

        public static readonly DependencyProperty HideCloseButtonProperty = DependencyProperty.RegisterAttached("HideCloseButton", typeof(bool), ownerType, new FrameworkPropertyMetadata(false, HideCloseButtonChangedCallback));

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHideCloseButton(Window obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return (bool)obj.GetValue(HideCloseButtonProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHideCloseButton(Window obj, bool value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            obj.SetValue(HideCloseButtonProperty, value);
        }

        static void HideCloseButtonChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                bool hideCloseButton = (bool)e.NewValue;
                if (hideCloseButton && !GetIsHiddenCloseButton(window))
                {
                    if (!window.IsLoaded)
                    {
                        window.Loaded += LoadedDelegate;
                    }
                    else
                    {
                        HideCloseButton(window);
                    }
                    SetIsHiddenCloseButton(window, true);
                }
                else if (!hideCloseButton && GetIsHiddenCloseButton(window))
                {
                    if (!window.IsLoaded)
                    {
                        window.Loaded -= LoadedDelegate;
                    }
                    else
                    {
                        ShowCloseButton(window);
                    }
                    SetIsHiddenCloseButton(window, false);
                }
            }
        }

        static readonly RoutedEventHandler LoadedDelegate = (sender, args) =>
        {
            if (sender is Window == false)
                return;
            Window w = (Window)sender;
            HideCloseButton(w);
            w.Loaded -= LoadedDelegate;
        };

        static void HideCloseButton(Window w)
        {
            IntPtr hwnd = new WindowInteropHelper(w).Handle;
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE) & ~NativeMethods.WS_SYSMENU);
        }

        static void ShowCloseButton(Window w)
        {
            IntPtr hwnd = new WindowInteropHelper(w).Handle;
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE) | NativeMethods.WS_SYSMENU);
        }

        #endregion

        #region IsHiddenCloseButton (readonly attached property)

        static readonly DependencyPropertyKey IsHiddenCloseButtonKey = DependencyProperty.RegisterAttachedReadOnly("IsHiddenCloseButton", typeof(bool), ownerType, new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsHiddenCloseButtonProperty = IsHiddenCloseButtonKey.DependencyProperty;

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetIsHiddenCloseButton(Window obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return (bool)obj.GetValue(IsHiddenCloseButtonProperty);
        }

        static void SetIsHiddenCloseButton(Window obj, bool value)
        {
            obj.SetValue(IsHiddenCloseButtonKey, value);
        }

        #endregion
    }
}