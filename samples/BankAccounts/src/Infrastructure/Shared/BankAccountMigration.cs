using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankAccounts.Infrastructure.Shared
{
    public static class BankAccountMigration
    {
        public static async Task<IHost> MigrateAsync(this IHost webHost, CancellationToken cancellationToken)
        {
            using var serviceScope = webHost.Services.CreateScope();

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<BankAccountDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);

            //var seedDataServices = serviceScope.ServiceProvider.GetServices<ISeedData>();

            //foreach (var seedDataService in seedDataServices.OrderBy(s => s.Order))
            //{
            //    await seedDataService.SeedDataAsync(cancellationToken);
            //}

            return webHost;
        }
    }
}
