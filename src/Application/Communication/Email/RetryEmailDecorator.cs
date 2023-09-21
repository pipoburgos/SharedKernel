#if !NET40
using SharedKernel.Application.Logging;

namespace SharedKernel.Application.Communication.Email;

/// <summary>  </summary>
public class RetryEmailDecorator : IEmailSender
{
    private readonly ICustomLogger<RetryEmailDecorator> _logger;
    private readonly IEmailSender _emailSender;
    private readonly IEmailSender? _emailSenderSaver;

    /// <summary>  </summary>
    protected int MaxRetries { get; } = 5;

    /// <summary>  </summary>
    public RetryEmailDecorator(
        ICustomLogger<RetryEmailDecorator> logger,
        IEmailSender emailSender,
        IEmailSender? emailSenderSaver)
    {
        _logger = logger;
        _emailSender = emailSender;
        _emailSenderSaver = emailSenderSaver;
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
            var attempts = 0;
            while (attempts < MaxRetries)
            {
                try
                {
                    _logger.Info($"Sending email with retries - attempt {attempts} of {MaxRetries} retries.");
                    await _emailSender.SendEmailAsync(email, cancellationToken);
                    attempts = MaxRetries;
                }
                catch (Exception)
                {
                    attempts++;

                    if (attempts == MaxRetries)
                    {
                        if (_emailSenderSaver != default)
                            await _emailSenderSaver.SendEmailAsync(email, cancellationToken);

                        throw;
                    }

                    var delay = new Random().Next(500, 2_000);
                    await Task.Delay(delay, cancellationToken);
                }
            }

        }
    }
}

#endif