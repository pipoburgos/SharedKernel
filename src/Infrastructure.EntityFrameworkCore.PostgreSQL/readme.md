The following code demonstrates basic usage of Shared Kernel Infastructure Entity Framework Core Sql Server.

```cs
namespace XXX.Infrastructure
{
    public static class XXXModule
    {
        public static IServiceCollection AddSharedKernelXXXModule(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName)
        {
            return services
                .AddEntityFrameworkCorePostgreSql<XXXDbContext, IXXXUnitOfWork>(configuration, connectionStringName);
        }
    }
}
```