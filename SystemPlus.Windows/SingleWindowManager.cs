using System.Windows;

namespace SystemPlus.Windows
{
    /// <summary>
    /// Manages windows, keeping one open at a time
    /// </summary>
    public class SingleWindowManager : ISingleWindowManager
    {
        ISingleWindow instance;

        public void Show(ISingleWindow window)
        {
            // close any existing window
            Close();

            if (window.Visibility != Visibility.Visible)
            {
                window.Visibility = Visibility.Visible;
                window.Focus();
            }
            else
                window.Show();

            instance = window;
        }

        public void Close()
        {
            if (instance != null)
            {
                instance.Close();
                instance = null;
            }
        }
    }

    public interface ISingleWindowManager
    {
        void Show(ISingleWindow window);

        void Close();
    }

    public interface ISingleWindow
    {
        bool Focus();
        void Close();
        void Show();
        Visibility Visibility { get; set; }
    }
}