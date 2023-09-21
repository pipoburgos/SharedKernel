using HealthChecks.Network.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Communication.Email;

namespace SharedKernel.Infrastructure.MailKit.Communication.Email.MailKitSmtp;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddMailKitSmtp(this IServiceCollection services, IConfiguration configuration)
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
            }, "Smtp", tags: new[] { "Smtp" });


        return services
            .AddOptions()
            .Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)))
            .AddTransient<IEmailSender, MailKitSmtpEmailSender>();
    }
}
