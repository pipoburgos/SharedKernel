The following code demonstrates basic usage of Shared Kernel Infastructure.

```cs
namespace XXX.Infrastructure;

public static class XXXModule
{
    public static IServiceCollection AddSharedKernelXXXModule(this IServiceCollection services,
        IConfiguration configuration, string connectionStringName)
    {
        return services
            .AddSharedKernel()
            .AddInMemoryCache()
            .AddDomainEventsSubscribers(typeof(InfrastructureAssembly), typeof(DomainAssembly)
            .AddCommandsHandlers(typeof(ApplicationAssembly))
            .AddInMemoryCommandBus()
            .AddQueriesHandlers(typeof(InfrastructureAssembly))
            .AddInMemoryQueryBus()
            .AddActiveDirectory()
            .AddApplicationServices()
            .AddDomainServices()
            .AddRepositories();
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddTransient<SampleApplicationService>();
    }
        
    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services
            .AddTransient<SampleDomainService>();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<IXXXRepository, XXXMongoRepository>();
            .AddTransient<IXXXRepository, XXXEntityFrameworkCoreRepository>();
    }
}

```