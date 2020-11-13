using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Integration.Tests.Shared;
using System;
using System.Threading.Tasks;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Serializers;
using Xunit;

namespace SharedKernel.Integration.Tests.Caching
{
    public class RedisCacheHelperTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddTransient<IBinarySerializer, BinarySerializer>()
                .AddDistributedRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6379";
                options.InstanceName = "shared.kernel";
            });
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
            Func<Task<Guid>> funcionGeneraValor = () =>
            {
                contador++;
                return Task.FromResult(id);
            };

            var savingAndGetting = inMemoryCacheHelper.GetOrCreateAsync("prueba", funcionGeneraValor);

            var getting = inMemoryCacheHelper.GetOrCreateAsync("prueba", funcionGeneraValor);

            Assert.Equal(id, await savingAndGetting);
            Assert.Equal(id, await getting);
            Assert.Equal(1, contador);

            inMemoryCacheHelper.Remove("prueba");
            var n3 = await inMemoryCacheHelper.GetOrCreateAsync("prueba", funcionGeneraValor);

            Assert.Equal(id, n3);
            Assert.Equal(2, contador);
        }
    }
}
