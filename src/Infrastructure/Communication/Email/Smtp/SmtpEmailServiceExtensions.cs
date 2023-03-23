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
            services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
            return services
                .AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>))
                .AddTransient<IEmailSender, SmtpEmailSender>();
        }
    }
}
