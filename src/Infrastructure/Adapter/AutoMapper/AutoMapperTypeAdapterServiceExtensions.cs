using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Adapter;
using System.Reflection;

namespace SharedKernel.Infrastructure.Adapter.AutoMapper
{
    /// <summary>  </summary>
    public static class AutoMapperTypeAdapterServiceExtensions
    {
        /// <summary>  </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapperSharedKernel(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services
                .AddTransient<ITypeAdapter, AutoMapperTypeAdapter>()
                .AddTransient<ITypeAdapterFactory>(s => new AutoMapperTypeAdapterFactory(s.GetRequiredService<IConfigurationProvider>()))
                .AddAutoMapper(assemblies);
        }

        /// <summary>  </summary>
        /// <returns></returns>
        public static void UseAutoMapperSharedKernel(ITypeAdapterFactory typeAdapterFactory)
        {
            TypeAdapterFactory.SetCurrent(typeAdapterFactory);
        }
    }
}
