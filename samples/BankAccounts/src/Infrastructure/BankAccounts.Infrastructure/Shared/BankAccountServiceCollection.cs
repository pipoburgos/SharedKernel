using BankAccounts.Application.Shared.UnitOfWork;
using BankAccounts.Domain.BankAccounts.Repository;
using BankAccounts.Domain.Services;
using BankAccounts.Infrastructure.BankAccounts;
using BankAccounts.Infrastructure.Shared.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Communication.Email.Smtp;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore;

namespace BankAccounts.Infrastructure.Shared
{
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
            return serviceCollection;
        }

        private static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
            IConfiguration configuration, string connectionStringName)
        {
            serviceCollection
                .AddSmtp(configuration);

            // Repositories
            serviceCollection.AddTransient<IBankAccountRepository, MongoBankAccountRepository>();

            // Unit of work
            serviceCollection.AddScoped<IBankAccountUnitOfWork, BankAccountDbContext>();
            serviceCollection.AddEntityFrameworkCoreSqlServer<BankAccountDbContext>(configuration, connectionStringName);

            return serviceCollection;
        }
    }
}
