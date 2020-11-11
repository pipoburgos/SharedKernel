using Microsoft.Extensions.Caching.Memory;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Integration.Tests.Shared;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Logging;
using Xunit;

namespace SharedKernel.Integration.Tests.Caching
{
    public class InMemoryCacheHelperTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddMemoryCache();
        }

        [Fact]
        public async Task TestCache()
        {
            var log = GetService<ICustomLogger<InMemoryCacheHelper>>();
            var memoryCache = GetService<IMemoryCache>();

            var inMemoryCacheHelper = new InMemoryCacheHelper(memoryCache, log);

            var id = Guid.NewGuid();
            var contador = 0;
            Func<Task<Guid>> funcionGeneraValor = () =>
            {
                contador++;
                return Task.FromResult(id);
            };

            var n = await inMemoryCacheHelper.GetOrCreateAsync("prueba", funcionGeneraValor);

            Assert.Equal(id, n);
            Assert.Equal(1, contador);


            var n2 = await inMemoryCacheHelper.GetOrCreateAsync("prueba", funcionGeneraValor);

            Assert.Equal(id, n2);
            Assert.Equal(1, contador);

            inMemoryCacheHelper.Remove("prueba");
            var n3 = await inMemoryCacheHelper.GetOrCreateAsync("prueba", funcionGeneraValor);

            Assert.Equal(id, n3);
            Assert.Equal(2, contador);
        }
    }
}
