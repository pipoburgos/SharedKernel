namespace SharedKernel.Application.Communication.Email;

/// <summary>  </summary>
public class OutboxMail : Mail
{
    /// <summary> . </summary>
    protected OutboxMail()
    {
    }

    /// <summary> . </summary>
    public OutboxMail(Guid id, string to, string? subject, string? body = default, string? from = default,
        List<string>? emailsBcc = default, List<MailAttachment>? attachments = default) : base(to, subject, body, from,
        emailsBcc, attachments)
    {
        Id = id;
        Pending = true;
    }

    /// <summary> . </summary>
    public OutboxMail(Guid id, List<string> to, string? subject, string? body = default, string? from = default,
        List<string>? emailsBcc = default, List<MailAttachment>? attachments = default) : base(to, subject, body, from,
        emailsBcc, attachments)
    {
        Id = id;
        Pending = true;
    }

    /// <summary>  </summary>
    public Guid Id { get; set; }

    /// <summary>  </summary>
    public bool Pending { get; set; }
}
