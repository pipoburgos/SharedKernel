The following code demonstrates basic usage of Shared Kernel Infastructure Mongo.

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
        public static IServiceCollection AddXXXModule(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName)
        {
            return services
                .AddMongo(configuration);
        }
    }
}
```