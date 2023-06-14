using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data.Dapper;
using SharedKernel.Infrastructure.Data.Dapper.Queries;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Testing.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.Dapper.Queries
{
    [Collection("DockerHook")]
    public class SqlServerDapperQueryProviderTests : InfrastructureTestCase<FakeStartup>
    {
        protected override string GetJsonFile()
        {
            return "Data/Dapper/Queries/appsettings.sqlServer.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("SharedKernelSqlServer3");

            return services
                .AddDbContext<SharedKernelDbContext>(options => options.UseSqlServer(connection!), ServiceLifetime.Transient)
                .AddDapperSqlServer(Configuration, "SharedKernelSqlServer");
        }

        [Fact]
        public async Task ExecuteQuery()
        {
            await Regenerate();
            var result = await GetRequiredService<DapperQueryProvider>()
                .ExecuteQueryFirstOrDefaultAsync<int>("SELECT 1");

            Assert.Equal(1, result);
        }

        private async Task Regenerate(CancellationToken cancellationToken = default)
        {
            await using var dbContext = GetRequiredService<SharedKernelDbContext>();
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
    }
}
