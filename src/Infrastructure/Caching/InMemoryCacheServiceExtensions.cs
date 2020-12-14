using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Caching;

namespace SharedKernel.Infrastructure.Caching
{
    public static class InMemoryCacheServiceExtensions
    {
        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            return services
                .AddMemoryCache()
                .AddTransient<ICacheHelper, InMemoryCacheHelper>();
        }
    }
}
