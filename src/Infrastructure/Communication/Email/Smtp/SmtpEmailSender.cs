using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Settings;
using SharedKernel.Infrastructure.Exceptions;

namespace SharedKernel.Infrastructure.Communication.Email.Smtp
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtp;

        public SmtpEmailSender(
            IOptionsService<SmtpSettings> emailSettings)
        {
            _smtp = emailSettings.Value;
        }

        public Task SendEmailAsync(string subject, string message, EmailAttachment attachment = null)
        {
            return SendEmailAsync(_smtp.SenderName, subject, message, attachment);
        }

        public async Task SendEmailAsync(string email, string subject, string message, EmailAttachment attachment = null)
        {
            if(string.IsNullOrWhiteSpace(_smtp.Password))
                throw new EmailException(nameof(ExceptionCodes.SMT_PASS_EMPTY));

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtp.Sender),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            if (attachment != default)
            {
                if(!attachment.Filename.Contains("."))
                    throw new EmailException(nameof(ExceptionCodes.EMAIL_ATTACH_EXT));

                mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachment.Data), attachment.Filename));
            }

            mailMessage.To.Add(email);

            using var client = new SmtpClient(_smtp.MailServer, _smtp.MailPort)
            {
                EnableSsl = _smtp.RequireSsl,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_smtp.SenderName, _smtp.Password)
            };
            await client.SendMailAsync(mailMessage);
        }
    }
}
