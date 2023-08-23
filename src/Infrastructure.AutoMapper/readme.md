The following code demonstrates basic usage of Shared Kernel Infastructure.

```cs
namespace XXX.Infrastructure
{
    public static class XXXModule
    {
        public static IServiceCollection AddXXXModule(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName)
        {
            return services
                .AddAutoMapper(new XXXAutoMapperProfile(), typeof(InfrastructureAssembly).Assembly,
                    typeof(DomainAssembly).Assembly;
        }

    }
}
```