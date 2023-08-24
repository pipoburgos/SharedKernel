using Community.Microsoft.Extensions.Caching.PostgreSql;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Dapper.Data;
using SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL.Caching;
using SharedKernel.Testing.Infrastructure;
using Xunit;

namespace SharedKernel.Integration.Tests.Caching.DistributedCache.PostgreSql
{
    [Collection("DockerHook")]
    public class PostgreSqlCacheHelperTests : InfrastructureTestCase<FakeStartup>
    {
        protected override string GetJsonFile()
        {
            return "Caching/DistributedCache/PostgreSql/appsettings.postgreSql.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddDapperPostgreSql(Configuration.GetSection(nameof(PostgreSqlCacheOptions) + ":ConnectionString").Value!)
                .AddPostgreSqlDistributedCache(Configuration);
        }

        [Fact]
        public async Task TestCache()
        {
            var distributedCache = GetService<IDistributedCache>();

            var binarySerializer = GetService<IBinarySerializer>();

            var inMemoryCacheHelper = new DistributedCacheHelper(distributedCache, binarySerializer);

            inMemoryCacheHelper.Remove("prueba");

            var id = Guid.NewGuid();
            var contador = 0;

            Task<Guid> FuncionGeneraValor()
            {
                contador++;
                return Task.FromResult(id);
            }

            var savingAndGetting = inMemoryCacheHelper.GetOrCreateAsync("prueba", FuncionGeneraValor);

            var getting = inMemoryCacheHelper.GetOrCreateAsync("prueba", FuncionGeneraValor);

            Assert.Equal(id, await savingAndGetting);
            Assert.Equal(id, await getting);
            Assert.Equal(1, contador);

            inMemoryCacheHelper.Remove("prueba");
            var n3 = await inMemoryCacheHelper.GetOrCreateAsync("prueba", FuncionGeneraValor);

            Assert.Equal(id, n3);
            Assert.Equal(2, contador);
        }
    }
}
