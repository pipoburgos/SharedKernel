//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

//internal sealed class DbContextMigrateHosted<TDbContext> : IHostedService where TDbContext : DbContext
//{
//    private readonly IServiceProvider _serviceProvider;

//    public DbContextMigrateHosted(IServiceProvider serviceProvider)
//    {
//        _serviceProvider = serviceProvider;
//    }

//    public async Task StartAsync(CancellationToken cancellationToken)
//    {
//        using var scope = _serviceProvider.CreateScope();
//        await using var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
//        await dbContext.Database.MigrateAsync(cancellationToken);
//    }

//    public Task StopAsync(CancellationToken cancellationToken)
//    {
//        return Task.CompletedTask;
//    }
//}