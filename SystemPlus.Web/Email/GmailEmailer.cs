using System.Net;
using System.Net.Mail;

namespace SystemPlus.Web.Email
{
    public class GmailEmailer : EmailSender
    {
        public GmailEmailer(string accountEmail, string accountPassword, string defaultFromName)
            : base(accountEmail, accountPassword, accountEmail, defaultFromName)
        {

        }

        protected override SmtpClient GetClient()
        {
            return new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(AccountEmail, AccountPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 30000,
            };
        }
    }
}
