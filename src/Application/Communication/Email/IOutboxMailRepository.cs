namespace SharedKernel.Application.Communication.Email;

/// <summary> . </summary>
public interface IOutboxMailRepository
{
    /// <summary> . </summary>
    Task<List<OutboxMail>> GetPendingMails(CancellationToken cancellationToken);

    /// <summary> . </summary>
    Task Add(OutboxMail outboxMail, CancellationToken cancellationToken);

    /// <summary> . </summary>
    Task Update(OutboxMail outboxMail, CancellationToken cancellationToken);
}
