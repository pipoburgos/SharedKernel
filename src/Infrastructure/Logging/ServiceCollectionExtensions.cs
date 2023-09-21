using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Logging;

namespace SharedKernel.Infrastructure.Logging;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddSharedKernelLogging(this IServiceCollection services)
    {
        return services
            .AddLogging()
            .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>));
        //.AddTransient<ICustomLogger, DefaultCustomLogger>();
    }
}
