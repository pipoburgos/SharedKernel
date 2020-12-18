using System.Threading.Tasks;

namespace SharedKernel.Application.Communication.Email
{
    /// <summary>
    /// Email sender manager
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email to default email from
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <param name="message">Message in HTML</param>
        /// <param name="attachment">File attachemnt</param>
        /// <returns></returns>
        Task SendEmailAsync(string subject, string message, EmailAttachment attachment = null);

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="email">From</param>
        /// <param name="subject">Subject</param>
        /// <param name="message">Message in HTML</param>
        /// <param name="attachment">File attachemnt</param>
        /// <returns></returns>
        Task SendEmailAsync(string email, string subject, string message, EmailAttachment attachment = null);
    }
}
