using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data.Dapper;
using SharedKernel.Infrastructure.Data.Dapper.Queries;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.Dapper.Queries
{
    public class DapperQueryProviderTests : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Data/Dapper/Queries/appsettings.sqlServer.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddDapperSqlServer<SharedKernelDbContext>(Configuration, "SharedKernelSqlServer");
        }

        [Fact]
        public async Task ExecuteQuery()
        {
            var result = await GetRequiredService<DapperQueryProvider<SharedKernelDbContext>>()
                .ExecuteQueryFirstOrDefaultAsync<int>("SELECT 1");

            Assert.Equal(1, result);
        }
    }
}
