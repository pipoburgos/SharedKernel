using System.Threading.Tasks;

namespace SharedKernel.Application.Communication.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string subject, string message, EmailAttachment attachment = null);

        Task SendEmailAsync(string email, string subject, string message, EmailAttachment attachment = null);
    }
}
