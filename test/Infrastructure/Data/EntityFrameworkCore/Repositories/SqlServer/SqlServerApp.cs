using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.Data;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.SqlServer;


public class SqlServerApp : InfrastructureTestCase<FakeStartup>
{
    protected override string GetJsonFile()
    {
        return "Data/EntityFrameworkCore/Repositories/SqlServer/appsettings.sqlServer.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddSharedKernelEntityFrameworkCoreSqlServerDbContext<SharedKernelEntityFrameworkDbContext>(
                Configuration.GetConnectionString("QueryProviderConnectionString")!)
            .AddHostedService<LoadTestData>();
    }

    private class LoadTestData : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public LoadTestData(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var dbContext = _serviceProvider
                .GetRequiredService<SharedKernelEntityFrameworkDbContext>();

            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);

            var repository = new EfCoreUserRepository(dbContext);

            var tasks = new List<Task>();
            for (var i = 0; i < 3; i++)
            {
                var roberto = UserMother.Create(parent: UserMother.Create());

                for (var j = 0; j < 2; j++)
                {
                    roberto.AddAddress(AddressMother.Create());
                }

                for (var j = 0; j < 3; j++)
                {
                    roberto.AddEmail(EmailMother.Create());
                }

                tasks.Add(repository.AddAsync(roberto, cancellationToken));
            }

            await Task.WhenAll(tasks);
            await repository.SaveChangesAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}