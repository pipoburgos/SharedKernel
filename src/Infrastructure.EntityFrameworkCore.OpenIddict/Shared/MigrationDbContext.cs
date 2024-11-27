using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Shared;

/// <summary> . </summary>
internal class MigrationDbContext<TDbContext> : IHostedService where TDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationDbContext(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        await using var authDbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        //await authDbContext.Database.EnsureDeletedAsync(cancellationToken);
        await authDbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
