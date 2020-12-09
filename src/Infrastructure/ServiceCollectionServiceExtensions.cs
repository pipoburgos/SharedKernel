using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.ActiveDirectory;
using SharedKernel.Application.Adapter;
using SharedKernel.Application.Caching;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Security.Cryptography;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Settings;
using SharedKernel.Application.System;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Events;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Security;
using SharedKernel.Infrastructure.ActiveDirectory;
using SharedKernel.Infrastructure.AutoMapper;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Communication.Email.Smtp;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Commands.InMemory;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Cqrs.Queries.InMemory;
using SharedKernel.Infrastructure.Data.Dapper.Queries;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.Queries;
using SharedKernel.Infrastructure.Data.FileSystem;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.Events.InMemory;
using SharedKernel.Infrastructure.Events.RabbitMq;
using SharedKernel.Infrastructure.Events.Redis;
using SharedKernel.Infrastructure.HealthChecks;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.Security;
using SharedKernel.Infrastructure.Security.Cryptography;
using SharedKernel.Infrastructure.Serializers;
using SharedKernel.Infrastructure.Settings;
using SharedKernel.Infrastructure.System;
using SharedKernel.Infrastructure.Validators;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure
{
    public static class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddSharedKernel(this IServiceCollection services)
        {
            services.AddLogging();

#if NET461
            services.AddTransient(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
#endif
            services.AddHealthChecks()
                .AddCheck("google.com", new PingHealthCheck("google.com", 200), tags: new[] { "system", "ping", "google.com" })
                .AddCheck("bing.com", new PingHealthCheck("bing.com", 200), tags: new[] { "system", "ping", "bing.com" })
                .AddCheck<RamHealthcheck>("RAM", tags: new[] { "system", "RAM" })
                .AddCheck<CpuHealthCheck>("CPU", tags: new[] { "system", "CPU" });

            return services
                .AddHostedService<QueuedHostedService>()
                .AddScoped<IFileSystemUnitOfWorkAsync, FileSystemUnitOfWork>()
                .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>()
                .AddTransient(typeof(DapperQueryProvider<>))
                .AddTransient(typeof(EntityFrameworkCoreQueryProvider<>))
                .AddTransient(typeof(ICustomLogger<>), typeof(DefaultCustomLogger<>))
                .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>))
                .AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>))
                .AddTransient<IActiveDirectoryService, ActiveDirectoryService>()
                .AddTransient<IAuditableService, AuditableService>()
                .AddTransient<IBase64, Base64>()
                .AddTransient<IBinarySerializer, BinarySerializer>()
                .AddTransient<ICulture, ThreadUiCulture>()
                .AddTransient<ICustomLogger, DefaultCustomLogger>()
                .AddTransient<IDateTime, MachineDateTime>()
                .AddTransient<IDirectoryRepositoryAsync, DirectoryRepositoryAsync>()
                .AddTransient<IEncryptionHexHelper, EncryptionHexHelper>()
                .AddTransient<IFileRepositoryAsync, FileRepositoryAsync>()
                .AddTransient<IGuid, GuidGenerator>()
                .AddTransient<IIdentityService, HttpContextAccessorIdentityService>()
                .AddTransient<IJsonSerializer, NewtonsoftSerializer>()
                .AddTransient<IModuleTransactionAsync, ModuleTransactionAsync>()
                .AddTransient<IRandomNumberGenerator, RandomNumberGenerator>()
                .AddTransient<ISemaphore, CustomSemaphore>()
                .AddTransient<ISha256, Sha256>()
                .AddTransient<IStreamHelper, StreamHelper>()
                .AddTransient<IStringHelper, StringHelper>()
                .AddTransient<ITripleDes, TripleDes>()
                .AddTransient<ITypeAdapterFactory, AutoMapperTypeAdapterFactory>()
                .AddTransient<IWeb, WebUtils>();
        }

        public static IServiceCollection AddInMemmoryCache(this IServiceCollection services)
        {
            return services
                .AddMemoryCache()
                .AddTransient<ICacheHelper, InMemoryCacheHelper>();
        }

        public static IServiceCollection AddRedisDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddRedis(GetRedisConfiguration(configuration), tags: new[] { "cache", "Redis" });

            return services
                .AddStackExchangeRedisCache(opt =>
                {
                    opt.Configuration = GetRedisConfiguration(configuration);
                })
                .AddTransient<IBinarySerializer, BinarySerializer>()
                .AddTransient<ICacheHelper, DistributedCacheHelper>();
        }

        public static IServiceCollection AddSmtp(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
            return services
                .AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>))
                .AddTransient<IEmailSender, SmtpEmailSender>();
        }

        public static IServiceCollection AddInMemmoryQueryBus(this IServiceCollection services)
        {
            return services
                .AddTransient<ExecuteMiddlewaresService>()
                .AddTransient<IQueryBus, InMemoryQueryBus>();
        }

        public static IServiceCollection AddInMemmoryCommandBus(this IServiceCollection services)
        {
            return services
                .AddTransient<ExecuteMiddlewaresService>()
                .AddTransient<ICommandBus, InMemoryCommandBus>();
        }

        private static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>))
                .AddTransient<ExecuteMiddlewaresService>()
                .AddTransient<DomainEventMediator>()
                .AddTransient<DomainEventsInformation>()
                .AddTransient<DomainEventSubscribersInformation>()
                .AddTransient<DomainEventJsonSerializer>()
                .AddTransient<DomainEventJsonDeserializer>();
        }

        public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services)
        {
            return services
                .AddEventBus()
                .AddTransient<IEventBus, InMemoryEventBus>();
        }

        public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfigParams>(configuration.GetSection("RabbitMq"));

            services
                .AddHealthChecks()
                .AddRabbitMQ(
                    sp => sp.CreateScope().ServiceProvider.GetRequiredService<RabbitMqConnectionFactory>().Connection(),
                    tags: new[] {"RabbitMq"});

            return services
                .AddHostedService<RabbitMqEventBusConfiguration>()
                .AddEventBus()
                //.AddTransient<MsSqlEventBus, MsSqlEventBus>() // Failover
                .AddTransient<IEventBus, RabbitMqEventBus>()
                .AddTransient<RabbitMqPublisher>()
                .AddTransient<RabbitMqConnectionFactory>()
                .AddTransient<RabbitMqDomainEventsConsumer>();
        }

        public static IServiceCollection AddRedisEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddHostedService<RedisConsumer>()
                .AddEventBus()
                .AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(GetRedisConfiguration(configuration)))
                .AddTransient<IEventBus, RedisEventBus>();
        }

        private static string GetRedisConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("RedisCacheOptions:Configuration").Value;
        }

        public static IServiceCollection AddSqlServer<TContext>(this IServiceCollection services,
            IConfiguration configuration, string connectionStringName) where TContext : DbContext
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString(connectionStringName), "SELECT 1;", "Sql Server",
                    HealthStatus.Unhealthy, new[] {"db", "sql", "SqlServer"});

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