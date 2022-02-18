using BankAccounts.Application.Shared;
using BankAccounts.Application.Shared.UnitOfWork;
using BankAccounts.Domain.BankAccounts.Repository;
using BankAccounts.Domain.Services;
using BankAccounts.Infrastructure.BankAccounts;
using BankAccounts.Infrastructure.Shared.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Communication.Email.Smtp;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.Data.Dapper;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.RetryPolicies;
using SharedKernel.Infrastructure.System;

namespace BankAccounts.Infrastructure.Shared
{
    /// <summary>
    /// Configurar la inyección de dependencias
    /// </summary>
    public static class BankAccountServiceCollection
    {
        public static IServiceCollection AddBankAccounts(this IServiceCollection serviceCollection,
            IConfiguration configuration, string connectionStringName)
        {
            return serviceCollection
                .AddSharedKernel()
                .AddDomain()
                .AddApplication()
                .AddInfrastructure(configuration, connectionStringName);
        }

        private static IServiceCollection AddDomain(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<BankTransferService>();

            return serviceCollection;
        }

        private static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddCommandsHandlers(typeof(IBankAccountUnitOfWork))
                .AddQueriesHandlers(typeof(BankAccountDbContext))
                .AddDomainEventsSubscribers(typeof(IBankAccountUnitOfWork).Assembly);
        }

        private static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
            IConfiguration configuration, string connectionStringName)
        {
            serviceCollection
                .AddSmtp(configuration);


            serviceCollection
                .AddFromMatchingInterface(typeof(IBankAccountRepository), typeof(EntityFrameworkBankAccountRepository),
                    typeof(IBankAccountUnitOfWork));

            // Repositories
            //serviceCollection.AddTransient<IBankAccountRepository, EntityFrameworkBankAccountRepository>();

            // Unit of work
            serviceCollection.AddScoped<IBankAccountUnitOfWork, BankAccountDbContext>();
            serviceCollection.AddEntityFrameworkCoreSqlServer<BankAccountDbContext>(configuration, connectionStringName);

            // Dapper
            serviceCollection.AddDapperSqlServer(configuration, connectionStringName);

            serviceCollection
                .AddTransient(typeof(IMiddleware<>), typeof(ValidationMiddleware<>))
                .AddTransient(typeof(IMiddleware<,>), typeof(ValidationMiddleware<,>))

                .AddTransient<IRetriever, PollyRetriever>()
                .AddTransient<IRetryPolicyExceptionHandler, BankAccountRetryPolicyExceptionHandler>()

                .AddTransient(typeof(IMiddleware<>), typeof(RetryPolicyMiddleware<>))
                .AddTransient(typeof(IMiddleware<,>), typeof(RetryPolicyMiddleware<,>))


                .AddTransient(typeof(IMiddleware<>), typeof(TimerMiddleware<>))
                .AddTransient(typeof(IMiddleware<,>), typeof(TimerMiddleware<,>));

            return serviceCollection;
        }
    }
}
