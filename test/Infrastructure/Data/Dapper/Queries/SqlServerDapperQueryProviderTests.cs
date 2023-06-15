using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
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
            var connection = Configuration.GetConnectionString("DapperConnectionString");

            return services
                .AddDbContext<SharedKernelDbContext>(options => options.UseSqlServer(connection!), ServiceLifetime.Transient)
                .AddDapperSqlServer(Configuration, "SharedKernelSqlServer");
        }

        [Fact]
        public async Task ExecuteQuery()
        {
            var dbContext = await Regenerate();
            var user = UserMother.Create();
            dbContext.Set<User>().Add(user);
            await dbContext.SaveChangesAsync();
            var result = await GetRequiredService<DapperQueryProvider>()
                .ExecuteQueryFirstOrDefaultAsync<int>($"SELECT COUNT(*) FROM User WHERE Id = {user.Id}");

            result.Should().Be(1);
        }

        private async Task<DbContext> Regenerate(CancellationToken cancellationToken = default)
        {
            await using var dbContext = GetRequiredService<SharedKernelDbContext>();
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);

            return dbContext;
        }
    }
}
