using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Settings;
using SharedKernel.Infrastructure.Settings;

namespace SharedKernel.Infrastructure.Communication.Email.Smtp
{
    /// <summary>  </summary>
    public static class SmtpEmailServiceExtensions
    {
        /// <summary>  </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSmtp(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpSettings = new SmtpSettings();
            configuration.GetSection(nameof(SmtpSettings)).Bind(smtpSettings);

            // todo not working
            //services
            //    .AddHealthChecks()
            //    .AddSmtpHealthCheck(setup =>
            //    {
            //        setup.Host = smtpSettings.MailServer;
            //        setup.Port = smtpSettings.MailPort;
            //        setup.ConnectionType = smtpSettings.RequireSsl ? SmtpConnectionType.SSL :
            //            smtpSettings.RequireTls ? SmtpConnectionType.TLS : SmtpConnectionType.AUTO;
            //        setup.LoginWith(smtpSettings.User, smtpSettings.Password);
            //        setup.AllowInvalidRemoteCertificates = true;
            //    }, "Smtp", tags: new[] { "Smtp" });


            return services
                .AddOptions()
                .Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)))
                .AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>))
                .AddTransient<IEmailSender, SmtpEmailSender>();
        }
    }
}
