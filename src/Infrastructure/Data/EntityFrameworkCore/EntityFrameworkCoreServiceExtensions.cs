using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.Logging;
using SharedKernel.Application.System;
using SharedKernel.Domain.Security;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.Queries;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.Security;
using SharedKernel.Infrastructure.System;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityFrameworkCoreServiceExtensions
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
        public static IServiceCollection AddEntityFrameworkCoreSqlServer<TContext>(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);

            services.AddHealthChecks()
                .AddSqlServer(connectionString, "SELECT 1;", "Sql Server EFCore",
                    HealthStatus.Unhealthy, new[] { "DB", "Sql", "SqlServer" });

            services.AddCommonDataServices();

            services.AddDbContext<TContext>(s => s
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging(), serviceLifetime);

#if NET461
            services.AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
            services.AddDbContextFactory<TContext>(lifetime: serviceLifetime);
#endif

            return services;
        }

        /// <summary>
        /// Add service PostgreSQL into IServiceCollection
        /// </summary>
        public static IServiceCollection AddEntityFrameworkCorePostgreSql<TContext>(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString(connectionStringName);

            services.AddHealthChecks()
                .AddNpgSql(connectionString, "SELECT 1;", null, "Postgre EFCore");

            services.AddCommonDataServices();

            services.AddDbContext<TContext>(p => p
                .UseNpgsql(connectionString)
                .EnableSensitiveDataLogging(), serviceLifetime);

            return services;
        }

        private static void AddCommonDataServices(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddTransient(typeof(EntityFrameworkCoreQueryProvider<>))
                .AddTransient<IIdentityService, HttpContextAccessorIdentityService>()
                .AddTransient<IDateTime, MachineDateTime>()
                .AddTransient<IGuid, GuidGenerator>()
                .AddTransient<IAuditableService, AuditableService>();
        }
    }
}
