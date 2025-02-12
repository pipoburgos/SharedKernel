using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankAccounts.Infrastructure.Shared;

public class BankAccountMigration : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public BankAccountMigration(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<BankAccountDbContext>();
        return dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}