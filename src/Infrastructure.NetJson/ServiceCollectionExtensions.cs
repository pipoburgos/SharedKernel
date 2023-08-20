using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.NetJson;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddNetJsonSerializer(this IServiceCollection services)
    {
        return services
            .RemoveAll<IJsonSerializer>()
            .AddTransient<IJsonSerializer, NetJsonSerializer>();
    }
}
