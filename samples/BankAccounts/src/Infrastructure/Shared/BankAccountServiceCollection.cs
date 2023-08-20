using BankAccounts.Application.Shared;
using BankAccounts.Application.Shared.UnitOfWork;
using BankAccounts.Domain.BankAccounts.Repository;
using BankAccounts.Domain.Services;
using BankAccounts.Infrastructure.BankAccounts;
using BankAccounts.Infrastructure.BankAccounts.Commands.Validators;
using BankAccounts.Infrastructure.Shared.Data;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Communication.Email.Smtp;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.Dapper.Data;
using SharedKernel.Infrastructure.EntityFrameworkCore.Requests.Middlewares;
using SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.FluentValidation;
using SharedKernel.Infrastructure.FluentValidation.Requests.Middlewares;
using SharedKernel.Infrastructure.Polly.Requests.Middlewares;
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
            .AddDomainEventsSubscribers(typeof(IBankAccountUnitOfWork), typeof(BankTransferService))
            .AddCommandsHandlers(typeof(IBankAccountUnitOfWork))
            .AddQueriesHandlers(typeof(BankAccountDbContext))
            .AddValidators(typeof(CreateBankAccountValidator));
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration, string connectionStringName)
    {
        return services
            .AddSmtp(configuration)
            .AddFromMatchingInterface(ServiceLifetime.Transient, typeof(IBankAccountRepository),
                typeof(EntityFrameworkBankAccountRepository), typeof(IBankAccountUnitOfWork))
            .AddEntityFrameworkCoreSqlServer<BankAccountDbContext>(configuration.GetConnectionString(connectionStringName)!)
            .AddScoped<IBankAccountUnitOfWork>(s => s.GetRequiredService<BankAccountDbContext>())
            .AddDapperSqlServer(configuration, connectionStringName)
            .AddEntityFrameworkFailoverMiddleware<BankAccountDbContext>()
            .AddValidationMiddleware()
            .AddRetryPolicyMiddleware<BankAccountRetryPolicyExceptionHandler>(configuration)
            .AddTimerMiddleware();
    }
}
