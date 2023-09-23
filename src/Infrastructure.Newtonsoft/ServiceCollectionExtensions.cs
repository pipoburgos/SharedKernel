using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.Newtonsoft;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddNewtonsoftSerializer(this IServiceCollection services)
    {
        return services
            .RemoveAll<IJsonSerializer>()
            .AddTransient<IJsonSerializer, NewtonsoftSerializer>();
    }

#if !NETFRAMEWORK && !NETSTANDARD
    /// <summary>  </summary>
    public static IMvcBuilder AddSharedKernelNewtonsoftJson(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.AddNewtonsoftJson(options =>
        {
            var settings = NewtonsoftSerializer.GetOptions(NamingConvention.CamelCase);
            options.SerializerSettings.DateTimeZoneHandling = settings.DateTimeZoneHandling;
            options.SerializerSettings.ContractResolver = settings.ContractResolver;
            options.SerializerSettings.NullValueHandling = settings.NullValueHandling;
            options.SerializerSettings.ConstructorHandling = settings.ConstructorHandling;
        });
    }
#endif
}
