using System;
using System.ComponentModel;
using System.Threading;

namespace SystemPlus.ComponentModel
{
    public interface IProgressToken : INotifyPropertyChanged
    {
        event ProgressTokenCancelledHandler Cancelled;

        #region Properties

        /// <summary>
        /// Handle exceptions called from token
        /// </summary>
        ProgressExceptionHandler? ExceptionHandler { get; set; }

        /// <summary>
        /// Title of current task
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Status of current task
        /// </summary>
        string Status { get; set; }

        /// <summary>
        /// Indicates if task is indeterminate
        /// </summary>
        bool IsIndeterminate { get; set; }

        /// <summary>
        /// Progress in percent of task
        /// </summary>
        double Progress { get; set; }

        ProgressTokenState State { get; set; }
        DateTime StartTime { get; }

        #endregion

        #region Private methods

        void UpdateStatus(string status, params object[] args);
        void UpdateProgress(double value, double max);

        /// <summary>
        /// Put token in error state
        /// </summary>
        void SetError(Exception ex);

        /// <summary>
        /// Requests cancellation
        /// </summary>
        void Cancel();

        /// <summary>
        /// Throws error if cancellation requested
        /// </summary>
        void ThrowIfCancellationRequested();

        /// <summary>
        /// Resets token to non cancelled state
        /// </summary>
        void ResetCancel();

        void HandleError(Exception ex);

        void Dispose();

        #endregion
    }
}