using System;

namespace SystemPlus.ComponentModel
{
    /// <summary>
    /// Delegate for handling exceptions
    /// </summary>
    public delegate void ProgressExceptionHandler(IProgressToken sender, Exception ex);
}