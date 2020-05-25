using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SystemPlus.Web.Email
{
    /// <summary>
    /// Sends emails
    /// Note: Max emails sent per day on office 365 is 10,000
    /// </summary>
    public abstract class EmailSender : IEmailSender
    {
        public string AccountEmail { get; }
        protected string AccountPassword { get; }

        public string DefaultFromEmail { get; }
        public string DefaultFromName { get; }

        public EmailSender(string accountEmail, string accountPassword, string defaultFromEmail, string defaultFromName)
        {
            AccountEmail = accountEmail;
            AccountPassword = accountPassword;
            DefaultFromEmail = defaultFromEmail;
            DefaultFromName = defaultFromName;
        }

        public Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml)
        {
            return SendEmailAsync(toEmail, DefaultFromEmail, DefaultFromName, subject, body, isHtml);
        }

        public async Task SendEmailAsync(string toEmail, string fromEmail, string fromName, string subject, string body, bool isHtml)
        {
            using MailMessage message = new MailMessage();
            message.To.Add(toEmail);
            message.From = new MailAddress(fromEmail, fromName);

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;

            await SendEmailAsync(message);
        }

        public async Task SendEmailAsync(MailMessage message)
        {
            try
            {
                using SmtpClient smtp = GetClient();
                await smtp.SendMailAsync(message);
            }
            catch (Exception)
            {
                //string toEmail = message.To.FirstOrDefault()?.Address;

                //if (toEmail != null)
                //    ex.AddData("Email", toEmail);

                throw;
            }
        }

        protected abstract SmtpClient GetClient();
    }
}
