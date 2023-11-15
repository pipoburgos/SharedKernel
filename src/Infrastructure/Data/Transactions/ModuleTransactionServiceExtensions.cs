using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Transactions;

namespace SharedKernel.Infrastructure.Data.Transactions;

/// <summary>
/// 
/// </summary>
public static class ModuleTransactionServiceExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddTransactions(this IServiceCollection services)
    {
        return services
            .AddTransient<IModuleTransactionAsync, ModuleTransactionAsync>();
    }
}