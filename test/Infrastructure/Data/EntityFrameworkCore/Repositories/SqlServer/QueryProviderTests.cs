using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.Queries;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Integration.Tests.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.SqlServer
{
    [Collection("DockerHook")]
    public class QueryProviderTests : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Data/EntityFrameworkCore/Repositories/SqlServer/appsettings.sqlServer.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("SharedKernelSqlServer2");
            return services
                .AddTransient<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>()
                .AddDbContextFactory<SharedKernelDbContext>(options => options.UseSqlServer(connection));
        }


        [Fact]
        public async Task SaveRepositoryNameChanged()
        {
            await LoadTestDataAsync(CancellationToken.None);

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var tasks = new List<Task<List<User>>>();
            for (var i = 0; i < 11; i++)
            {
                tasks.Add(queryProvider.GetQuery<User>().Where(u => u.Name.Length != 23).ToListAsync());
            }

            var queries = await Task.WhenAll(tasks);
            Assert.Equal(11, queries.Length);
        }

        private async Task LoadTestDataAsync(CancellationToken cancellationToken)
        {
            await using var dbContext = GetService<IDbContextFactory<SharedKernelDbContext>>().CreateDbContext();
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);

            var repository = new UserEfCoreRepository(dbContext);

            var tasks = new List<Task>();
            for (var i = 0; i < 11; i++)
            {
                var roberto = UserMother.Create();

                for (var j = 0; j < 10; j++)
                {
                    roberto.AddAddress(AddressMother.Create());
                }

                for (var j = 0; j < 5; j++)
                {
                    roberto.AddEmail(EmailMother.Create());
                }

                tasks.Add(repository.AddAsync(roberto, cancellationToken));
            }

            await Task.WhenAll(tasks);
            await repository.SaveChangesAsync(cancellationToken);
        }
    }
}
