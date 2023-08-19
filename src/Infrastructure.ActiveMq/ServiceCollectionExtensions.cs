using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Infrastructure.ActiveMq;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    internal static IServiceCollection AddActiveMqHealthChecks(this IServiceCollection services, IConfiguration configuration,
        string name, params string[] tags)
    {
        //services
        //    .AddHealthChecks()
        //    .AddApacheMq(brokerUri, "Apache ActiveMq Event Bus", tags: new[] { "Event Bus", "Apache", "ActiveMq" });

        services
            .AddActiveMqOptions(configuration);
        //.AddHealthChecks()
        //.AddRedis(sp => sp.GetRequiredService<IOptions<RedisCacheOptions>>().Value.Configuration!, name, tags: tagsList);

        return services;
    }

    /// <summary>  </summary>
    internal static IServiceCollection AddActiveMqOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddOptions()
            .Configure<ActiveMqConfiguration>(configuration.GetSection(nameof(ActiveMqConfiguration)));
    }
}
