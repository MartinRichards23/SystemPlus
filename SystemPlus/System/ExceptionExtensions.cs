using System.Diagnostics.CodeAnalysis;
using SystemPlus.Text;

namespace SystemPlus
{
    /// <summary>
    /// Extensions for Exceptions
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Generates an extended string of the exeception including the Data key/values
        /// </summary>
        public static string ToString(this Exception ex, bool includeData)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            StringBuilder sb = new StringBuilder(ex.ToString());

            if (includeData)
            {
                if (ex.Data.Count > 0)
                {
                    sb.AppendLine();
                    sb.AppendLine("Data:");

                    foreach (object? key in ex.Data.Keys)
                    {
                        if (key == null)
                            continue;

                        object? val = ex.Data[key];

                        if (val == null)
                            continue;

                        sb.AppendLine("{0}: {1}", key, val);
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Safely adds data to the exception
        /// </summary>
        public static void AddData(this Exception ex, string key, object data)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));
            if (key == null)
                return;
            if (data == null)
                return;

            try
            {
                ex.Data.Add(key, data);
            }
            catch
            {
                // swallow all errors
            }
        }

        /// <summary>
        /// Gets the message and the messages of the inner exceptions
        /// </summary>
        public static string AllMessage(this Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            string message = ex.Message;

            if (ex.InnerException != null)
            {
                message += "\r\n\r\n" + ex.InnerException.AllMessage();
            }

            return message;
        }

        /// <summary>
        /// Sets the result object on the exception
        /// Result object must be serialisable
        /// </summary>
        public static void SetResult(this Exception ex, object result)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            ex.Data.Add("Result", result);
        }

        /// <summary>
        /// Gets the result object from the exception if there is one
        /// </summary>
        [return: MaybeNull]
        public static T GetResult<T>(this Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            object? obj = ex.Data["Result"];

            if (obj is T t)
                return t;

            return default;
        }
    }
}
