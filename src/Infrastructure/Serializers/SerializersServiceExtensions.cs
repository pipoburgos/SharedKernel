using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.Serializers
{
    public static class SerializersServiceExtensions
    {
        public static IServiceCollection AddNetJsonSerializer(this IServiceCollection services)
        {
            return services
                .AddTransient<IJsonSerializer, NetJsonSerializer>();
        }

        public static IServiceCollection AddNewtonsoftSerializer(this IServiceCollection services)
        {
            return services
                .AddTransient<IJsonSerializer, NewtonsoftSerializer>();
        }
    }
}
