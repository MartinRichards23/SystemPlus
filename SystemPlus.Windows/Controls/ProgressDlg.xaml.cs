using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SystemPlus.Windows.Controls
{
    public partial class ProgressDlg
    {
        readonly CancellationTokenSource cancelToken = new CancellationTokenSource();
        DateTime startTime;
        DispatcherTimer? timer;
        readonly bool showTimer;
        TimeSpan timeSpan;

        public ProgressDlg(Window owner)
            : this(owner, "", false, false)
        {
        }

        public ProgressDlg(string title, bool isIndeterminate, bool allowCancel, bool showTimer = false)
            : this(Application.Current.MainWindow, title, isIndeterminate, allowCancel, showTimer)
        {
        }

        public ProgressDlg(Window owner, string title, bool isIndeterminate, bool allowCancel, bool showTimer = false)
        {
            InitializeComponent();

            Title = title;
            Status = string.Empty;

            IsIndeterminate = isIndeterminate;
            AllowCancel = allowCancel;

            if (owner != null)
                Owner = owner;
            else if (Application.Current != null && Application.Current.MainWindow != null)
                Owner = Application.Current.MainWindow;

            if (Owner != null)
                Icon = Owner.Icon;

            this.showTimer = showTimer;
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            startTime = DateTime.Now;

            if (showTimer)
            {
                timer = new DispatcherTimer();
                timer.Tick += Timer_Tick;
                timer.Interval = TimeSpan.FromSeconds(0.9);
                timer.Start();
            }
        }

        void Timer_Tick(object? sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            timeSpan = now - startTime;

            txtTime.Text = TimeSpanExtensions.FormatTimeSpan(timeSpan);
        }

        public string Status
        {
            get { return txtStatus.Text; }
            set { txtStatus.Text = value; }
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

        public DateTime StartTime
        {
            get { return startTime; }
        }

        public TimeSpan TimeSpan
        {
            get { return timeSpan; }
        }

        public CancellationTokenSource CancelToken
        {
            get { return cancelToken; }
        }

        public void UpdateTitle(string value)
        {
            Dispatcher.BeginInvoke((Action)delegate { Title = value; });
        }

        public void UpdateStatus(string value)
        {
            Dispatcher.BeginInvoke((Action)delegate { txtStatus.Text = value; });
        }

        public void UpdatePercent(double percent)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                progressBar1.Value = percent;
                txtPercent.Content = percent.ToString("0.#", CultureInfo.CurrentCulture) + "%";
            });
        }

        public void UpdatePercent(int value, int max)
        {
            UpdatePercent((value / (double)max) * 100);
        }

        public void UpdateIsIndeterminate(bool value)
        {
            Dispatcher.BeginInvoke((Action)delegate { IsIndeterminate = value; });
        }

        public void ThrowIfCancellationRequested()
        {
            cancelToken.Token.ThrowIfCancellationRequested();
        }

        void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            cancelToken.Cancel();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Owner != null)
                Owner.Focus();
        }
    }
}