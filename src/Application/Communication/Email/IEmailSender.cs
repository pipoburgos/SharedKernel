namespace SharedKernel.Application.Communication.Email;

/// <summary> Email sender manager. </summary>
public interface IEmailSender
{
    /// <summary>  </summary>
    bool Sender { get; }

    /// <summary> Sends an email to default email from. </summary>
    Task SendEmailAsync(Mail email, CancellationToken cancellationToken);

    /// <summary> Sends emails to default email from. </summary>
    Task SendEmailAsync(IEnumerable<Mail> emails, CancellationToken cancellationToken);
}
