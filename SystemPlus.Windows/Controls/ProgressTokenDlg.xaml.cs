using System.ComponentModel;
using System.Windows;
using SystemPlus.ComponentModel;

namespace SystemPlus.Windows.Controls
{
    public partial class ProgressTokenDlg
    {
        public ProgressTokenDlg() : this(Application.Current?.MainWindow)
        {
        }

        public ProgressTokenDlg(Window? owner)
        {
            InitializeComponent();

            Token = new ProgressToken();
            DataContext = Token;

            if (owner != null)
                Owner = owner;
            else if (Application.Current != null && Application.Current.MainWindow != null)
                Owner = Application.Current.MainWindow;
            if (Owner != null)
                Icon = Owner.Icon;
        }

        public bool IsIndeterminate
        {
            get { return progressBar1.IsIndeterminate; }
            set
            {
                progressBar1.IsIndeterminate = value;
                txtPercent.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool AllowCancel
        {
            get { return btnCancel.Visibility == Visibility.Visible; }
            set { btnCancel.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        }

        public ProgressToken Token
        {
            get;
        }

        public void ThrowIfCancellationRequested()
        {
            Token.ThrowIfCancellationRequested();
        }

        void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Token.Cancel();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Owner != null)
                Owner.Focus();
        }
    }
}