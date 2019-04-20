using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using SystemPlus.ComponentModel;

namespace SystemPlus.Windows.Controls
{
    public partial class ProgressTokenDlg
    {
        public ProgressTokenDlg() : this(Application.Current?.MainWindow)
        {
        }

        public ProgressTokenDlg(Window owner)
        {
            InitializeComponent();
            
            Token = new ProgressToken();
            this.DataContext = Token;
              
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
            set { btnCancel.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        }
        
        public ProgressToken Token
        {
            get;
        }
                        
        public void ThrowIfCancellationRequested()
        {
            Token.CancelToken.ThrowIfCancellationRequested();
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
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