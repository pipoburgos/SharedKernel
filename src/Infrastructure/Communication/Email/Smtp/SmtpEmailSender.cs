using Microsoft.Extensions.Options;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Exceptions;
using System.Net;
using System.Net.Mail;

namespace SharedKernel.Infrastructure.Communication.Email.Smtp;

/// <summary> . </summary>
public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _smtp;

    /// <summary> . </summary>
    public SmtpEmailSender(
        IOptions<SmtpSettings> emailSettings)
    {
        _smtp = emailSettings.Value;
    }

    /// <summary> . </summary>
    public bool Sender => true;

    /// <summary> . </summary>
    public Task SendEmailAsync(Mail email, CancellationToken cancellationToken)
    {
        return SendEmailAsync(new List<Mail> { email }, cancellationToken);
    }

    /// <summary> . </summary>
    public async Task SendEmailAsync(IEnumerable<Mail> emails, CancellationToken cancellationToken)
    {
        var mails = CreateMailMessages(emails);

        using var client = new SmtpClient(_smtp.MailServer, _smtp.MailPort);
        client.EnableSsl = _smtp.RequireSsl;

        if (!string.IsNullOrWhiteSpace(_smtp.User))
        {
            if (string.IsNullOrWhiteSpace(_smtp.Password))
                throw new EmailException(nameof(ExceptionCodes.SMT_PASS_EMPTY));

            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(_smtp.User, _smtp.Password);
        }

        var tasks = new List<Task>();
        foreach (var email in mails)
        {
#if NET6_0_OR_GREATER
            tasks.Add(client.SendMailAsync(email, cancellationToken));
#else
            tasks.Add(client.SendMailAsync(email));
#endif
        }


        await Task.WhenAll(tasks);
    }

    private List<MailMessage> CreateMailMessages(IEnumerable<Mail> emails)
    {
        var mails = new List<MailMessage>();
        foreach (var email in emails)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(email.From ?? _smtp.DefaultSender),
                Subject = email.Subject,
                Body = email.Body ?? string.Empty,
                IsBodyHtml = true
            };

            mailMessage.To.Add(string.Join(";", email.To));

            if (email.Attachments != default && email.Attachments.Any())
            {
                if (_smtp.MaxSendSize.HasValue && email.Attachments.Sum(a => a.ContentStream.Length) > _smtp.MaxSendSize)
                    throw new EmailException(nameof(ExceptionCodes.EMAIL_ATTACH_EXT));

                foreach (var attachment in email.Attachments)
                {
                    if (!attachment.Filename.Contains("."))
                        throw new EmailException(nameof(ExceptionCodes.EMAIL_ATTACH_EXT));

                    mailMessage.Attachments.Add(new Attachment(attachment.ContentStream, attachment.Filename));
                }
            }

            mails.Add(mailMessage);
        }

        return mails;
    }
}
