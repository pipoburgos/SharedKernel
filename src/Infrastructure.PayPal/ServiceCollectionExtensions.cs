using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.PayPal.Api;

namespace SharedKernel.Infrastructure.PayPal;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddPayPal(this IServiceCollection services, IConfiguration configuration, Action<PayPalOptions>? configure = null)
    {
        //var openIdOptions = new PayPalOptions();
        //configuration.GetSection(nameof(PayPalOptions)).Bind(openIdOptions);

        //services.Configure<PayPalOptions>(configuration.GetSection(nameof(PayPalOptions)));

        if (configure != null)
            services.Configure(configure);

        services.AddTransient<IPayPalClient, APIContext>();
        services.AddHttpClient("PayPal");
        return services;
    }
}
