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
    public static class DapperServiceExtensions
    {
        public static IServiceCollection AddDapperSqlServer<TContext>(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName) where TContext : DbContext
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString(connectionStringName), "SELECT 1;", "Sql Server Dapper",
                    HealthStatus.Unhealthy, new[] { "DB", "Sql", "SqlServer" });

            services.AddTransient(typeof(DapperQueryProvider<>));

            services.AddDbContext<TContext>(s => s
                .UseSqlServer(configuration.GetConnectionString(connectionStringName))
                .EnableSensitiveDataLogging(), ServiceLifetime.Transient);

#if NET461
            services.AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
            services.AddDbContextFactory<TContext>(lifetime: ServiceLifetime.Transient);
#endif

            return services;
        }
    }
}
