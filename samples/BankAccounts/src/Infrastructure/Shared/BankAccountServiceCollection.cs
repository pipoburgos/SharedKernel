using BankAccounts.Application;
using BankAccounts.Application.Shared.UnitOfWork;
using BankAccounts.Domain;
using BankAccounts.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Communication.Email.Smtp;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.Dapper.SqlServer.Data;
using SharedKernel.Infrastructure.Data;
using SharedKernel.Infrastructure.EntityFrameworkCore.Communication.Email;
using SharedKernel.Infrastructure.EntityFrameworkCore.Requests.Middlewares;
using SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.Data;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.FluentValidation;
using SharedKernel.Infrastructure.MailKit.Communication.Email.MailKitSmtp;
using SharedKernel.Infrastructure.Polly.Requests.Middlewares;
using SharedKernel.Infrastructure.Redis.Data;
using SharedKernel.Infrastructure.Requests.Middlewares;
using SharedKernel.Infrastructure.System;

namespace BankAccounts.Infrastructure.Shared;

/// <summary> Configurar la inyección de dependencias. </summary>
public static class BankAccountServiceCollection
{
    public static IServiceCollection AddBankAccounts(this IServiceCollection services,
        IConfiguration configuration, string connectionStringName)
    {
        return services
            .AddSharedKernel()
            .AddDomain()
            .AddApplication()
            .AddInfrastructure(configuration, connectionStringName);
    }

    private static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services
            .AddTransient<BankTransferService>();
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddSharedKernelDomainEventsSubscribers(typeof(BankAccountsApplicationAssembly), typeof(BankAccountsDomainAssembly))
            .AddSharedKernelCommandsHandlers(typeof(BankAccountsApplicationAssembly))
            .AddSharedKernelQueriesHandlers(typeof(BankAccountsInfrastructureAssembly))
            .AddSharedKernelFluentValidation(typeof(BankAccountsInfrastructureAssembly));
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration, string connectionStringName)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)!;
        return services
            .AddSharedKernelMailKitSmtp(configuration)
            .AddSharedKernelOutbox()
            .AddSharedKernelEntityFrameworkCoreOutboxMailRepository<BankAccountDbContext>()
            .AddSharedKernelFromMatchingInterface(ServiceLifetime.Transient, typeof(BankAccountsDomainAssembly),
                typeof(BankAccountsApplicationAssembly), typeof(BankAccountsInfrastructureAssembly))
            .AddSharedKernelEntityFrameworkCoreSqlServerDbContext<BankAccountDbContext>(connectionString, ServiceLifetime.Scoped)
            .AddSharedKernelRedisDbContext<BankAccountRedisDbContext>(configuration, ServiceLifetime.Scoped)
            .AddSharedKernelGlobalUnitOfWorkAsync<IBankAccountUnitOfWork, BankAccountGlobalUnitOfWorkAsync>()
            .AddSharedKernelDapperSqlServer(connectionString)
            .AddHostedService<BankAccountMigration>()
            .AddSharedKernelFailoverMiddleware()
            .AddSharedKernelEntityFrameworkCoreFailoverRepository<BankAccountDbContext>()
            .AddSharedKernelValidationMiddleware()
            .AddSharedKernelRetryPolicyMiddleware<BankAccountRetryPolicyExceptionHandler>(configuration)
            .AddSharedKernelTimerMiddleware();
    }
}