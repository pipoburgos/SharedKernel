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
    public static class EntityFrameworkCoreServiceExtensions
    {
        public static IServiceCollection AddEntityFrameworkCoreSqlServer<TContext>(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName) where TContext : DbContextBase
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString(connectionStringName), "SELECT 1;", "Sql Server EFCore",
                    HealthStatus.Unhealthy, new[] { "DB", "Sql", "SqlServer" });

            services
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddTransient(typeof(EntityFrameworkCoreQueryProvider<>))
                .AddTransient<IIdentityService, HttpContextAccessorIdentityService>()
                .AddTransient<IDateTime, MachineDateTime>()
                .AddTransient<IGuid, GuidGenerator>()
                .AddTransient<IAuditableService, AuditableService>();

            services.AddDbContext<TContext>(s => s
                .UseSqlServer(configuration.GetConnectionString(connectionStringName))
                .EnableSensitiveDataLogging());

#if NET461
            services.AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#else
            services.AddDbContextFactory<TContext>(lifetime: ServiceLifetime.Transient);
#endif

            return services;
        }
    }
}
