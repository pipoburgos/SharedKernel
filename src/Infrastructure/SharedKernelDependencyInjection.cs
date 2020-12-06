using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Infrastructure.Data.FileSystem;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.Events.InMemory;
using SharedKernel.Infrastructure.Events.RabbitMq;
using SharedKernel.Infrastructure.HealthChecks;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Infrastructure.Security;
using SharedKernel.Infrastructure.Security.Cryptography;
using SharedKernel.Infrastructure.Serializers;
using SharedKernel.Infrastructure.Settings;
using SharedKernel.Infrastructure.System;
using SharedKernel.Infrastructure.Validators;

namespace SharedKernel.Infrastructure
{
    public static class SharedKernelDependencyInjection
    {
        public static IServiceCollection AddSharedKernel(this IServiceCollection services)
        {
            services.AddLogging();

            services.AddHealthChecks()
                .AddCheck("google.com", new PingHealthCheck("google.com", 20))
                .AddCheck("bing.com", new PingHealthCheck("bing.com", 20))
                .AddCheck<SystemMemoryHealthcheck>("Memory");

            return services
                .AddHostedService<QueuedHostedService>()
                .AddScoped<IFileSystemUnitOfWorkAsync, FileSystemUnitOfWork>()
                .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>()
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

        public static IServiceCollection AddSmtp(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
            return services.AddTransient<IEmailSender, SmtpEmailSender>();
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

        public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services)
        {
            return services
                .AddTransient<ExecuteMiddlewaresService>()
                .AddScoped<IEventBus, InMemoryEventBus>()
                .AddScoped<DomainEventsInformation>()
                .AddScoped<DomainEventJsonDeserializer>();
        }

        public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfigParams>(configuration.GetSection("RabbitMq"));

            services
                .AddHealthChecks()
                .AddRabbitMQ(sp => sp.GetRequiredService<RabbitMqConfig>().Connection());

            return services
                //.AddScoped<MsSqlEventBus, MsSqlEventBus>() // Failover
                .AddTransient<ExecuteMiddlewaresService>()
                .AddScoped<IEventBus, RabbitMqEventBus>()
                .AddScoped<IEventBusConfiguration, RabbitMqEventBusConfiguration>()
                .AddScoped<RabbitMqPublisher>()
                .AddScoped<RabbitMqConfig>()
                .AddScoped<RabbitMqDomainEventsConsumer>()
                .AddScoped<DomainEventsInformation>()
                .AddScoped<DomainEventJsonDeserializer>();
        }
    }
}