using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Infrastructure.Data.Dapper.ConnectionFactory;
using SharedKernel.Infrastructure.Data.Dapper.Queries;

namespace SharedKernel.Infrastructure.Data.Dapper
{
    /// <summary>
    /// 
    /// </summary>
    public static class DapperServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="connectionStringName"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDapperSqlServer(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);

            services.AddHealthChecks()
                .AddSqlServer(connectionString!, "SELECT 1;", "Sql Server Dapper",
                    HealthStatus.Unhealthy, new[] { "DB", "Sql", "SqlServer" });

            services.AddTransient<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            services.Add(new ServiceDescriptor(typeof(DapperQueryProvider), typeof(DapperQueryProvider), serviceLifetime));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="connectionStringName"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDapperPostgreSql(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);

            services.AddHealthChecks()
                .AddNpgSql(connectionString!, "SELECT 1;", null, "Postgre Dapper");

            services.AddTransient<IDbConnectionFactory>(_ => new PostgreSqlConnectionFactory(connectionString));
            services.Add(new ServiceDescriptor(typeof(DapperQueryProvider), typeof(DapperQueryProvider), serviceLifetime));

            return services;
        }
    }
}
