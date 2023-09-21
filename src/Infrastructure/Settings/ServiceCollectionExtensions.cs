using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Settings;

namespace SharedKernel.Infrastructure.Settings;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddSharedKernelOptions(this IServiceCollection services)
    {
        return services
            .AddOptions()
            .AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>));
    }
}
