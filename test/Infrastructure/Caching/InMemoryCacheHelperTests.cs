using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Testing.Infrastructure;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Caching
{
    public class InMemoryCacheHelperTests : InfrastructureTestCase<FakeStartup>
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddInMemoryCache();
        }

        [Fact]
        public async Task TestCache()
        {
            var log = GetService<ICustomLogger<InMemoryCacheHelper>>();
            var memoryCache = GetService<IMemoryCache>();

            var inMemoryCacheHelper = new InMemoryCacheHelper(memoryCache, log);

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
