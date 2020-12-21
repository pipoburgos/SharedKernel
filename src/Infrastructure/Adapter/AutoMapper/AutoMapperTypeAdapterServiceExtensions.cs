using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Adapter;

namespace SharedKernel.Infrastructure.Adapter.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoMapperTypeAdapterServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProfile"></typeparam>
        /// <param name="services"></param>
        /// <param name="profile"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper<TProfile>(this IServiceCollection services, TProfile profile,
            params Assembly[] assemblies) where TProfile : Profile
        {
            TypeAdapterFactory.SetCurrent(new AutoMapperTypeAdapterFactory(profile));
            return services
                .AddTransient<ITypeAdapterFactory, AutoMapperTypeAdapterFactory>()
                .AddAutoMapper(assemblies);
        }
    }
}
