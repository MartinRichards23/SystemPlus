﻿using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace SystemPlus.ComponentModel
{
    /// <summary>
    /// Manages the status of a task
    /// </summary>
    public class ProgressToken : IDisposable, IProgressToken
    {
        #region Fields

        string title = string.Empty;
        string status = string.Empty;
        bool isIndeterminate;
        double progress;
        ProgressTokenState state = ProgressTokenState.Normal;
        readonly DateTime startTime = DateTime.UtcNow;
        CancellationTokenSource? cancelTokenSource;
        ProgressExceptionHandler? exceptionHandler;

        bool disposedValue;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event ProgressTokenCancelledHandler? Cancelled;

        #endregion

        public ProgressToken()
        {
            cancelTokenSource = new CancellationTokenSource();
        }

        public ProgressToken(CancellationTokenSource cancelTokenSource)
        {
            this.cancelTokenSource = cancelTokenSource;
        }

        #region Properties

        /// <summary>
        /// Handle exceptions called from token
        /// </summary>
        public ProgressExceptionHandler? ExceptionHandler
        {
            get { return exceptionHandler; }
            set { exceptionHandler = value; }
        }

        /// <summary>
        /// Title of current task
        /// </summary>
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Status of current task
        /// </summary>
        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        /// <summary>
        /// Indicates if task is indeterminate
        /// </summary>
        public bool IsIndeterminate
        {
            get { return isIndeterminate; }
            set
            {
                if (isIndeterminate != value)
                {
                    isIndeterminate = value;
                    OnPropertyChanged("IsIndeterminate");
                }
            }
        }

        /// <summary>
        /// Progress in percent of task
        /// </summary>
        public double Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    OnPropertyChanged("Progress");
                }
            }
        }

        public ProgressTokenState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged("State");
                }
            }
        }

        public DateTime StartTime
        {
            get { return startTime; }
        }

        #endregion

        #region Methods

        public void UpdateStatus(string status, params object[] args)
        {
            Status = string.Format(CultureInfo.CurrentCulture, status, args);
        }

        public void UpdateProgress(double value, double max)
        {
            Progress = value / max * 100;
        }

        /// <summary>
        /// Put token in error state
        /// </summary>
        public void SetError(Exception ex)
        {
            IsIndeterminate = false;
            Progress = 0;
            Status = ex?.Message ?? string.Empty;
        }

        /// <summary>
        /// Requests cancellation
        /// </summary>
        public void Cancel()
        {
            cancelTokenSource?.Cancel();

            OnCancelled();
        }

        /// <summary>
        /// Throws error if cancellation requested
        /// </summary>
        public void ThrowIfCancellationRequested()
        {
            cancelTokenSource?.Token.ThrowIfCancellationRequested();
        }

        /// <summary>
        /// Resets token to non cancelled state
        /// </summary>
        public void ResetCancel()
        {
            cancelTokenSource?.Dispose();

            cancelTokenSource = new CancellationTokenSource();
        }

        public void HandleError(Exception ex)
        {
            if (ex is OperationCanceledException)
                throw ex;

            exceptionHandler?.Invoke(this, ex);
        }

        protected virtual void OnCancelled()
        {
            Cancelled?.Invoke(this);
        }

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (cancelTokenSource != null)
                    {
                        cancelTokenSource.Dispose();
                        cancelTokenSource = null;
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Returnd an empty ProgressToken
        /// </summary>
        public static IProgressToken None
        {
            get { return new ProgressToken(); }
        }
    }

    public enum ProgressTokenState
    {
        Normal,
        Errored,
        Warned,
        Completed
    }

    public delegate void ProgressTokenCancelledHandler(IProgressToken token);
}