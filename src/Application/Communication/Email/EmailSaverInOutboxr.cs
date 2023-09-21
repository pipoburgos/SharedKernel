#if !NET40
using SharedKernel.Application.System;

namespace SharedKernel.Application.Communication.Email;

/// <summary>  </summary>
public class EmailSaverInOutboxr : IEmailSender
{
    private readonly IGuid _guid;
    private readonly IOutboxMailRepository _outboxMailRepository;

    /// <summary>  </summary>
    public EmailSaverInOutboxr(
        IGuid guid,
        IOutboxMailRepository? outboxMailRepository = default)
    {
        _guid = guid;
        _outboxMailRepository =
            outboxMailRepository ?? throw new NotImplementedException("IOutboxMailRepository not registered.");
    }

    /// <summary>  </summary>
    public bool Sender => false;

    /// <summary>  </summary>
    public Task SendEmailAsync(Mail email, CancellationToken cancellationToken)
    {
        return SendEmailAsync(new List<Mail> { email }, cancellationToken);
    }

    /// <summary>  </summary>
    public async Task SendEmailAsync(IEnumerable<Mail> emails, CancellationToken cancellationToken)
    {
        foreach (var email in emails)
        {
            await _outboxMailRepository.Add(
                new OutboxMail(_guid.NewGuid(), email.To, email.Subject, email.Body, email.From, email.EmailsBcc,
                    email.Attachments), cancellationToken);
        }
    }
}

#endif