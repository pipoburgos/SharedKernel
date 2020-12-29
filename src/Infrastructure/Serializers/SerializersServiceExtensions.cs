using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.Serializers
{
    /// <summary>
    /// 
    /// </summary>
    public static class SerializersServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNetJsonSerializer(this IServiceCollection services)
        {
            return services
                .AddTransient<IJsonSerializer, NetJsonSerializer>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNewtonsoftSerializer(this IServiceCollection services)
        {
            return services
                .AddTransient<IJsonSerializer, NewtonsoftSerializer>();
        }
    }
}
