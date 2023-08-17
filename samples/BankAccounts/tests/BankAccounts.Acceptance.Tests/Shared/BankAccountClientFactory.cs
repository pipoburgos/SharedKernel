using BankAccounts.Api;
using BankAccounts.Infrastructure.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Testing.Acceptance.WebApplication;

namespace BankAccounts.Acceptance.Tests.Shared
{
    public class BankAccountClientFactory : WebApplicationFactoryBase<Startup>
    {
        protected override DbContext CreateScopeReturnDbContext()
        {
            return Services.CreateScope().ServiceProvider.GetRequiredService<BankAccountDbContext>();
        }

        public DbContext CreateNewDbContext()
        {
            return Services.CreateScope().ServiceProvider.GetRequiredService<BankAccountDbContext>();
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureTestServices(services);
            services.RemoveAll<IEmailSender>().AddTransient(_ => Substitute.For<IEmailSender>());
        }
    }
}