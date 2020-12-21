using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Infrastructure.Data.Dapper.Queries;
#if NET461
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
#endif

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
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="connectionStringName"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDapperSqlServer<TContext>(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TContext : DbContext
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString(connectionStringName), "SELECT 1;", "Sql Server Dapper",
                    HealthStatus.Unhealthy, new[] { "DB", "Sql", "SqlServer" });

            services.AddTransient(typeof(DapperQueryProvider<>));

            services.AddDbContext<TContext>(s => s
                .UseSqlServer(configuration.GetConnectionString(connectionStringName))
                .EnableSensitiveDataLogging(), serviceLifetime);

#if NET461
            services.AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
            services.AddDbContextFactory<TContext>(lifetime: serviceLifetime);
#endif

            return services;
        }
    }
}
