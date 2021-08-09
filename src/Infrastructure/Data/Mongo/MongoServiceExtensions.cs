using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Infrastructure.Data.Mongo.Queries;

namespace SharedKernel.Infrastructure.Data.Mongo
{
    /// <summary>
    /// 
    /// </summary>
    public static class MongoServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));

            services.AddHealthChecks()
                .AddMongoDb(
                    configuration.GetSection(nameof(MongoSettings) + ":" + nameof(MongoSettings.ConnectionString))
                        .Value, "Mongo", HealthStatus.Unhealthy, new[] { "DB", "NoSql", "Mongo" });

            services.AddTransient<MongoQueryProvider>();

            return services;
        }
    }
}
