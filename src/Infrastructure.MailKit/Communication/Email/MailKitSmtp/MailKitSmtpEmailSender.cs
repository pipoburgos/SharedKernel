using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Exceptions;

namespace SharedKernel.Infrastructure.MailKit.Communication.Email.MailKitSmtp;

/// <summary>  </summary>
public class MailKitSmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _smtp;

    /// <summary>  </summary>
    public MailKitSmtpEmailSender(
        IOptions<SmtpSettings> emailSettings)
    {
        _smtp = emailSettings.Value;
    }

    /// <summary>  </summary>
    public bool Sender => true;

    /// <summary>  </summary>
    public Task SendEmailAsync(Mail email, CancellationToken cancellationToken)
    {
        return SendEmailAsync(new List<Mail> { email }, cancellationToken);
    }

    /// <summary>  </summary>
    public async Task SendEmailAsync(IEnumerable<Mail> emails, CancellationToken cancellationToken)
    {
        var mails = await CreateMimeMessages(emails, cancellationToken);

        using var client = new SmtpClient();

        await client.ConnectAsync(_smtp.MailServer, _smtp.MailPort, false, cancellationToken);

        if (!string.IsNullOrWhiteSpace(_smtp.User))
        {
            if (string.IsNullOrWhiteSpace(_smtp.Password))
                throw new EmailException(nameof(ExceptionCodes.SMT_PASS_EMPTY));

            await client.AuthenticateAsync(new SaslMechanismLogin(_smtp.User, _smtp.Password), cancellationToken);
        }

        var tasks = new List<Task>();
        foreach (var email in mails)
        {
            tasks.Add(client.SendAsync(email, cancellationToken));
        }
        await Task.WhenAll(tasks);

        //await client.DisconnectAsync(true, cancellationToken);
    }

    private async Task<List<MimeMessage>> CreateMimeMessages(IEnumerable<Mail> emails, CancellationToken cancellationToken)
    {
        var mails = new List<MimeMessage>();
        foreach (var email in emails)
        {
            var mailMessage = new MimeMessage
            {
                Subject = email.Subject,
                Body = new TextPart(TextFormat.Html) { Text = email.Body }
            };

            mailMessage.From.Add(new MailboxAddress(email.From ?? _smtp.DefaultSender,
                email.From ?? _smtp.DefaultSender));

            mailMessage.To.AddRange(email.To.Select(t => new MailboxAddress(t, t)));

            if (email.Attachments != default && email.Attachments.Any())
            {
                if (_smtp.MaxSendSize.HasValue && email.Attachments.Sum(a => a.ContentStream.Length) > _smtp.MaxSendSize)
                    throw new EmailException(nameof(ExceptionCodes.EMAIL_ATTACH_EXT));

                var builder = new BodyBuilder();
                foreach (var attachment in email.Attachments)
                {
                    if (!attachment.Filename.Contains("."))
                        throw new EmailException(nameof(ExceptionCodes.EMAIL_ATTACH_EXT));

                    await builder.Attachments.AddAsync(attachment.Filename, attachment.ContentStream,
                        cancellationToken); //, MimeKit.ContentType.Parse(MediaTypeNames.Application.Pdf));
                }
            }

            mails.Add(mailMessage);
        }

        return mails;
    }
}
