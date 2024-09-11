using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.Newtonsoft;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddNewtonsoftSerializer(this IServiceCollection services)
    {
        return services
            .RemoveAll<IJsonSerializer>()
            .AddTransient<IJsonSerializer, NewtonsoftSerializer>();
    }
}
