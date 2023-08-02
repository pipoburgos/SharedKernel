#if !NET461 && !NETSTANDARD2_1 && !NETCOREAPP3_1
using System;
#endif
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Security;
using SharedKernel.Application.System;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.Queries;
using SharedKernel.Infrastructure.Logging;
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
            return services.AddEntityFrameworkCoreSqlServer<TContext>(connectionString, serviceLifetime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddEntityFrameworkCoreSqlServer<TContext>(this IServiceCollection services,
            string connectionString, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext
        {
            services.AddHealthChecks()
                .AddSqlServer(connectionString!, "SELECT 1;", _ => { }, "Sql Server EFCore",
                    HealthStatus.Unhealthy, new[] { "DB", "Sql", "SqlServer" });

            services.AddCommonDataServices();

#if NET461 || NETSTANDARD2_1 || NETCOREAPP3_1

            services.AddDbContext<TContext>(s => s.UseSqlServer(connectionString), serviceLifetime);

            services.AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
            services.AddDbContext<TContext>(s => s.UseSqlServer(connectionString, e => e
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1), default)), serviceLifetime);

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
            return services.AddEntityFrameworkCorePostgreSql<TContext>(connectionString, serviceLifetime);
        }

        /// <summary>
        /// Add service PostgreSQL into IServiceCollection
        /// </summary>
        public static IServiceCollection AddEntityFrameworkCorePostgreSql<TContext>(this IServiceCollection services,
            string connectionString, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext
        {
            services.AddHealthChecks()
                .AddNpgSql(connectionString!, "SELECT 1;", null, "Postgre EFCore");

            services.AddCommonDataServices();

#if NET461 || NETSTANDARD2_1 || NETCOREAPP3_1

            services.AddDbContext<TContext>(p => p.UseNpgsql(connectionString), serviceLifetime);

            services.AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
            services.AddDbContext<TContext>(p => p.UseNpgsql(connectionString, e => e
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .EnableRetryOnFailure(5, TimeSpan.FromSeconds(1), default)), serviceLifetime);

            services.AddDbContextFactory<TContext>(lifetime: serviceLifetime);
#endif

            return services;
        }

        /// <summary>
        /// Register common Ef Core data services
        /// </summary>
        /// <param name="services"></param>
        private static void AddCommonDataServices(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddTransient(typeof(EntityFrameworkCoreQueryProvider<>))
                .AddTransient<IIdentityService, DefaultIdentityService>()
                .AddTransient<IDateTime, MachineDateTime>()
                .AddTransient<IGuid, GuidGenerator>()
                .AddTransient<IAuditableService, AuditableService>()
                .AddTransient<IValidatableObjectService, ValidatableObjectService>();
        }
    }
}
