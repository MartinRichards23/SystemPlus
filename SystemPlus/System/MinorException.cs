﻿namespace SystemPlus
{
    /// <summary>
    /// A non fatal exception that can doesn't need to be logged
    /// </summary>
    [Serializable]
    public class MinorException : Exception
    {
        public MinorException()
        {
        }

        public MinorException(string message)
            : base(message)
        {
        }

        public MinorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}