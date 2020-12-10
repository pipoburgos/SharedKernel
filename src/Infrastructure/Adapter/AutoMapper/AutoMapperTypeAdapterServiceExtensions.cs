using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Adapter;

namespace SharedKernel.Infrastructure.Adapter.AutoMapper
{
    public static class AutoMapperTypeAdapterServiceExtensions
    {
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
