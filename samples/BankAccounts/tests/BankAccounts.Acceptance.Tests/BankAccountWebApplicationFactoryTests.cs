using BankAccounts.Acceptance.Tests.Shared;
using BankAccounts.Api;
using BankAccounts.Infrastructure.Shared.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Testing.Acceptance.Tests;

namespace BankAccounts.Acceptance.Tests
{
    [Collection("Factory")]
    public class BankAccountWebApplicationFactoryTests : WebApplicationFactoryBaseTests<Startup>
    {
        private readonly BankAccountClientFactory _bankAccountClientFactory;

        protected override Startup CreateStartup(IConfiguration configuration)
        {
            return new Startup(configuration);
        }

        protected override void ConfigureServices(Startup startup, IServiceCollection services)
        {
            startup.ConfigureServices(services);
        }

        public BankAccountWebApplicationFactoryTests(BankAccountClientFactory bankAccountClientFactory)
        {
            _bankAccountClientFactory = bankAccountClientFactory;
        }

        [Fact]
        protected override void StartupWhenBuildServiceCollectionShouldDependenciesBeRegistered()
        {
            base.Startup_WhenBuildServiceCollection_ShouldDependenciesBeRegistered();
        }

        [Fact]
        protected override async Task SwaggerGenerateJsonOk()
        {
            await base.Swagger_TestGenerateJson(await _bankAccountClientFactory.CreateClientAsync<BankAccountDbContext>());
        }

        [Fact]
        protected override async Task AllIsHealthy()
        {
            await base.All_Is_Healthy(await _bankAccountClientFactory.CreateClientAsync<BankAccountDbContext>());
        }
    }
}
