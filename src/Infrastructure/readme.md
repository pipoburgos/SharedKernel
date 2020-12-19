The following code demonstrates basic usage of Shared Kernel Infastructure.

### appsettings.json for mongo connection

```json
{
  "MongoSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "Database": "ModuleName"
  }
}
```
```cs
namespace ModuleName.Infrastructure
{
    public static class ModuleNameModule
    {
        public static IServiceCollection AddModuleNameModule(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName)
        {
            return services
                .AddScoped<IPopulateDatabase, PopulateDatabase>()
                .AddAutoMapper(new ModuleNameAutoMapperProfile(), typeof(ApplicationCommandHandler).Assembly,
                    typeof(UserRegistered).Assembly,
                    typeof(ModuleNameDbContext).Assembly)
                .AddDomainEvents(typeof(UserRegistered))
                .AddDomainEventsSubscribers(typeof(UserRegisteredSusbcriber))
                .AddCommandsHandlers(typeof(ApplicationCommandHandler))
                .AddQueriesHandlers(typeof(ModuleNameDbContext))
                .AddDapperSqlServer<ModuleNameDbContext>(configuration, connectionStringName)
                .AddEntityFrameworkCoreSqlServer<ModuleNameDbContext>(configuration, connectionStringName)
                .AddMongo(configuration)
                .AddApplicationServices()
                .AddDomainServices()
                .AddRepositories();
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services;
        }
        
        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            return services
                .AddTransient<MakeModuleNameWithInvoice>();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<ICompanyRepository, CompanyEntityFrameworkCoreRepository>()
                .AddTransient<IInvoiceRepository, InvoiceEntityFrameworkCoreRepository>()
                .AddTransient<IUserRepository, UserEntityFrameworkCoreRepository>()
                .AddTransient<IModuleNameMadeRepository, ModuleNameMadeEntityFrameworkCoreRepository>();
        }
    }
}
```