using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayPal.V1.Shared;

namespace SharedKernel.Infrastructure.PayPal;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelPayPal(this IServiceCollection services, IConfiguration configuration, Action<PayPalOptions>? configure = null)
    {
        //var openIdOptions = new PayPalOptions();
        //configuration.GetSection(nameof(PayPalOptions)).Bind(openIdOptions);

        //services.Configure<PayPalOptions>(configuration.GetSection(nameof(PayPalOptions)));

        if (configure != null)
            services.Configure(configure);

        services.AddTransient<IPayPalClient, PayPalClient>();
        services.AddHttpClient("PayPal");
        return services;
    }
}
