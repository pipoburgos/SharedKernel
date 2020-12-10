using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Transactions;

namespace SharedKernel.Infrastructure.Data.Transactions
{
    public static class ModuleTransactionServiceExtensions
    {
        public static IServiceCollection AddTransactions(this IServiceCollection services)
        {
            return services
                .AddTransient<IModuleTransactionAsync, ModuleTransactionAsync>();
        }
    }
}
