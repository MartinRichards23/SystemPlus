using System;
using System.Collections.Generic;
using System.Text;
using SystemPlus.Web.Email;
using Microsoft.Extensions.Logging;

namespace SystemPlus.Web.Logging
{
    public class EmailLoggerProvider : ILoggerProvider
    {
        IEmailSender emailer;
        string address;

        public EmailLoggerProvider(IEmailSender emailer, string address)
        {
            this.emailer = emailer;
            this.address = address;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EmailLogger(emailer, address);
        }

        public void Dispose()
        {

        }
    }

    class EmailLogger : ILogger
    {
        IEmailSender emailer;
        string address;

        public EmailLogger(IEmailSender emailer, string address)
        {
            this.emailer = emailer;
            this.address = address;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Error;
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel >= LogLevel.Error && exception != null)
            {
                try
                {
                    await emailer.SendEmailAsync(address, "Error", exception.ToString(), false);
                }
                catch
                { }
            }
        }
    }
}
