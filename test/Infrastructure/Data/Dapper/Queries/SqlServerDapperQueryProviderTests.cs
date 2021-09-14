using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data.Dapper;
using SharedKernel.Infrastructure.Data.Dapper.Queries;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
using SharedKernel.Integration.Tests.Docker;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.Dapper.Queries
{
    [Collection("DockerHook")]
    public class SqlServerDapperQueryProviderTests : InfrastructureTestCase
    {
        public SqlServerDapperQueryProviderTests(DockerHook dockerHook)
        {
            dockerHook.Run();
        }

        protected override string GetJsonFile()
        {
            return "Data/Dapper/Queries/appsettings.sqlServer.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services.AddDapperSqlServer(Configuration, "SharedKernelSqlServer");
        }

        [Fact]
        public async Task ExecuteQuery()
        {
            var result = await GetRequiredService<DapperQueryProvider>()
                .ExecuteQueryFirstOrDefaultAsync<int>("SELECT 1");

            Assert.Equal(1, result);
        }
    }
}
