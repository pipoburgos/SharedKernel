using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Newtonsoft;
using Swashbuckle.AspNetCore.Newtonsoft;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SharedKernel.Api.Newtonsoft;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
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

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelSwaggerGenNewtonsoftSupport(this IServiceCollection services)
    {
        return services.Replace(
            ServiceDescriptor.Transient<ISerializerDataContractResolver>(_ =>
            {
                var settings = NewtonsoftSerializer.GetOptions(NamingConvention.CamelCase);
                return new NewtonsoftDataContractResolver(settings);
            }));
    }
}
