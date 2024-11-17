using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.ActiveDirectory;

namespace SharedKernel.Infrastructure.ActiveDirectory;

/// <summary>
/// 
/// </summary>
public static class ActiveDirectoryServiceExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSharedKernelActiveDirectory(this IServiceCollection services)
    {
        return services
            .AddTransient<IActiveDirectoryService, ActiveDirectoryService>();
    }
}