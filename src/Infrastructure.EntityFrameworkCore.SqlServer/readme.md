The following code demonstrates basic usage of Shared Kernel Infastructure Entity Framework Core Sql Server.

```cs
namespace XXX.Infrastructure
{
    public static class XXXModule
    {
        public static IServiceCollection AddXXXModule(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName)
        {
            return services
                .AddEntityFrameworkCoreSqlServer<XXXDbContext, IXXXUnitOfWork>(configuration, connectionStringName);
        }
    }
}
```