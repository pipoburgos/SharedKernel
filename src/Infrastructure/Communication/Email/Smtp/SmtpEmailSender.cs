using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Settings;
using SharedKernel.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Communication.Email.Smtp
{
    /// <summary>
    /// 
    /// </summary>
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailSettings"></param>
        public SmtpEmailSender(
            IOptionsService<SmtpSettings> emailSettings)
        {
            _smtp = emailSettings.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SendEmailAsync(Mail email, CancellationToken cancellationToken)
        {
            return SendEmailAsync(new List<Mail> { email }, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="EmailException"></exception>
        public async Task SendEmailAsync(IEnumerable<Mail> emails, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_smtp.Password))
                throw new EmailException(nameof(ExceptionCodes.SMT_PASS_EMPTY));

            try
            {

                var tasks = new List<Task>();


                using var client = new SmtpClient(_smtp.MailServer, _smtp.MailPort)
                {
                    EnableSsl = _smtp.RequireSsl,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(_smtp.SenderName, _smtp.Password)
                };

                foreach (var email in emails)
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(email.From ?? _smtp.Sender),
                        Subject = email.Subject,
                        Body = email.Body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(string.Join(";", email.To));

                    if (email.Attachments != default && email.Attachments.Any())
                    {
                        foreach (var attachment in email.Attachments)
                        {
                            if (!attachment.Filename.Contains("."))
                                throw new EmailException(nameof(ExceptionCodes.EMAIL_ATTACH_EXT));

                            mailMessage.Attachments.Add(new Attachment(attachment.ContentStream, attachment.Filename));
                        }
                    }

#if NET6_0_OR_GREATER
                    tasks.Add(client.SendMailAsync(mailMessage, cancellationToken));
#else
                    tasks.Add(client.SendMailAsync(mailMessage));
#endif
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                throw new EmailException(e);
            }
        }
    }
}
