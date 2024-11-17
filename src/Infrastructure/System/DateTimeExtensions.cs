using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.System;

namespace SharedKernel.Infrastructure.System;

/// <summary>
/// 
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSharedKernelMachineDateTime(this IServiceCollection services)
    {
        return services
            .AddTransient<IDateTime, MachineDateTime>();
    }
}