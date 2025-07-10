using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Mapper;
using System.Reflection;

namespace SharedKernel.Infrastructure.AutoMapper;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddSharedKernelAutoMapperSharedKernel(this IServiceCollection services, params Assembly[] assemblies)
    {
        return services
            .AddTransient<Application.Mapper.IMapper, AutoMapperAdapter>()
            .AddTransient<IMapperFactory>(s => new AutoMapperFactory(s.GetRequiredService<IConfigurationProvider>()))
            .AddAutoMapper(x =>
            {
                x.AddMaps(assemblies);
            });
    }

    /// <summary> . </summary>
    public static void UseAutoMapperSharedKernel(IMapperFactory typeAdapterFactory)
    {
        MapperFactory.SetCurrent(typeAdapterFactory);
    }
}
