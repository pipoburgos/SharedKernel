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
using SharedKernel.Infrastructure.Dapper.Data;
using SharedKernel.Infrastructure.Data;
using SharedKernel.Infrastructure.EntityFrameworkCore.Requests.Middlewares;
using SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.Data;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.FluentValidation;
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
            .AddDomainEventsSubscribers(typeof(BankAccountsApplicationAssembly), typeof(BankAccountsDomainAssembly))
            .AddCommandsHandlers(typeof(BankAccountsApplicationAssembly))
            .AddQueriesHandlers(typeof(BankAccountsInfrastructureAssembly))
            .AddFluentValidation(typeof(BankAccountsInfrastructureAssembly));
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration, string connectionStringName)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName)!;
        return services
            .AddSmtp(configuration)
            .AddFromMatchingInterface(ServiceLifetime.Transient, typeof(BankAccountsDomainAssembly),
                typeof(BankAccountsApplicationAssembly), typeof(BankAccountsInfrastructureAssembly))
            .AddEntityFrameworkCoreSqlServerDbContext<BankAccountDbContext>(connectionString, ServiceLifetime.Scoped)
            .AddRedisDbContext<BankAccountRedisDbContext>(configuration, ServiceLifetime.Scoped)
            .AddGlobalUnitOfWorkAsync<IBankAccountUnitOfWork, BankAccountGlobalUnitOfWorkAsync>()
            .AddDapperSqlServer(connectionString)
            .AddFailoverMiddleware()
            .AddEntityFrameworkCoreFailoverRepository<BankAccountDbContext>()
            .AddValidationMiddleware()
            .AddRetryPolicyMiddleware<BankAccountRetryPolicyExceptionHandler>(configuration)
            .AddTimerMiddleware();
    }
}