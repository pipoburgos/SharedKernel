The following code demonstrates basic usage of Shared Kernel Infastructure.

### appsettings.json for mongo connection

```json
{
  "MongoSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "Database": "XXX"
  }
}
```
```cs
namespace XXX.Infrastructure
{
    public static class XXXModule
    {
        public static IServiceCollection AddSharedKernelXXXModule(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName)
        {
            return services
                .AddDomainEvents(typeof(XXXEvent))
                .AddDomainEventsSubscribers(typeof(XXXEventSusbcriber))
                .AddCommandsHandlers(typeof(ApplicationCommandHandler))
                .AddQueriesHandlers(typeof(InfrastructureQueryHandler))
                .AddDapperSqlServer<XXXDbContext>(configuration, connectionStringName)
                .AddEntityFrameworkCoreSqlServer<XXXDbContext>(configuration, connectionStringName)
                .AddMongo(configuration)
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
}
```