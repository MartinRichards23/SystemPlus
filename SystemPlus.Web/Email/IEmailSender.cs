using System.Net.Mail;
using System.Threading.Tasks;

namespace SystemPlus.Web.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml);
        Task SendEmailAsync(string toEmail, string fromEmail, string fromName, string subject, string body, bool isHtml);
        Task SendEmailAsync(MailMessage message);
    }
}
