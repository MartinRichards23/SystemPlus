﻿using System;
using System.Text;
using SystemPlus.Text;

namespace SystemPlus
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Generates an extended string of the exeception including the Data key/values
        /// </summary>
        public static string ToString(this Exception ex, bool includeData)
        {
            StringBuilder sb = new StringBuilder(ex.ToString());

            if (includeData)
            {
                if (ex.Data.Count > 0)
                {
                    sb.AppendLine();
                    sb.AppendLine("Data:");

                    foreach (var key in ex.Data.Keys)
                    {
                        if (key == null)
                            continue;

                        object val = ex.Data[key];

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
            string message = ex.Message;

            if(ex.InnerException != null)
            {
                message += "\r\n\r\n"+ ex.InnerException.AllMessage();
            }

            return message;
        }

        /// <summary>
        /// Sets the result object on the exception
        /// Result object must be serialisable
        /// </summary>
        public static void SetResult(this Exception ex, object result)
        {
            ex.Data.Add("Result", result);
        }

        /// <summary>
        /// Gets the result object from the exception if there is one
        /// </summary>
        public static T GetResult<T>(this Exception ex)
        {
            object obj = ex.Data["Result"];

            if (obj is T)
                return (T)obj;

            return default(T);
        }
    }
}