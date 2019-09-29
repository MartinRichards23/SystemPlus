using Microsoft.Extensions.Logging;
using System;
using SystemPlus.Web.Email;

namespace SystemPlus.Web.Logging
{
    public class EmailLoggerProvider : ILoggerProvider
    {
        readonly IEmailSender emailer;
        readonly string address;

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
        readonly IEmailSender emailer;
        readonly string address;

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
