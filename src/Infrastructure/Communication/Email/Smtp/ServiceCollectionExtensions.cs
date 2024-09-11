using HealthChecks.Network.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Logging;

namespace SharedKernel.Infrastructure.Communication.Email.Smtp;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSmtp(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpSettings = new SmtpSettings();
        configuration.GetSection(nameof(SmtpSettings)).Bind(smtpSettings);

        services
            .AddHealthChecks()
            .AddSmtpHealthCheck(setup =>
            {
                setup.Host = smtpSettings.MailServer;
                if (smtpSettings.MailPort != default)
                {
                    setup.Port = smtpSettings.MailPort;
                    setup.ConnectionType = SmtpConnectionType.PLAIN;
                }
                else
                {
                    setup.ConnectionType = smtpSettings.RequireSsl ? SmtpConnectionType.SSL :
                        smtpSettings.RequireTls ? SmtpConnectionType.TLS : SmtpConnectionType.PLAIN;
                }
                if (!string.IsNullOrWhiteSpace(smtpSettings.User) && !string.IsNullOrWhiteSpace(smtpSettings.Password))
                    setup.LoginWith(smtpSettings.User!, smtpSettings.Password!);

                setup.AllowInvalidRemoteCertificates = true;
            }, "Smtp", tags: ["Smtp"]);


        return services
            .Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)))
            .AddTransient<SmtpEmailSender>()
            .AddTransient<IEmailSender, SmtpEmailSender>()
            .AddTransient<IEmailSender>(sp =>
                new RetryEmailDecorator(
                    sp.GetRequiredService<ICustomLogger<RetryEmailDecorator>>(),
                    sp.GetRequiredService<SmtpEmailSender>(),
                    default));
    }

    /// <summary> . </summary>
    public static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        return services
            .AddHostedService<OutboxHostedService>()
            .AddTransient<IEmailSender, EmailSaverInOutboxr>();
    }
}
