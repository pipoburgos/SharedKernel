using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SharedKernel.Infrastructure.Data.Mongo
{
    public static class MongoServiceExtensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));

            services.AddHealthChecks()
                .AddMongoDb(
                    configuration.GetSection(nameof(MongoSettings) + ":" + nameof(MongoSettings.ConnectionString))
                        .Value, name: "Mongo", HealthStatus.Unhealthy, new[] {"DB", "NoSql", "Mongo"});

            return services;
        }
    }
}
