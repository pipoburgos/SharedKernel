using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Caching;

namespace SharedKernel.Infrastructure.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public static class InMemoryCacheServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            return services
                .AddLogging()
                .AddMemoryCache()
                .AddTransient<ICacheHelper, InMemoryCacheHelper>();
        }
    }
}
