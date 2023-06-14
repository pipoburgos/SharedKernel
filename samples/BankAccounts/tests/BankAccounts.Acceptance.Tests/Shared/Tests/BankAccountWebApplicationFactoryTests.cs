using BankAccounts.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Testing.Acceptance.Tests;

namespace BankAccounts.Acceptance.Tests.Shared.Tests
{
    public class BankAccountWebApplicationFactoryTests : WebApplicationFactoryBaseTests<Startup>
    {
        protected override Startup CreateStartup(IConfiguration configuration)
        {
            return new Startup(configuration);
        }

        protected override void ConfigureServices(Startup startup, IServiceCollection services)
        {
            startup.ConfigureServices(services);
        }

        [Fact]
        protected override void StartupWhenBuildServiceCollectionShouldDependenciesBeRegistered()
        {
            base.Startup_WhenBuildServiceCollection_ShouldDependenciesBeRegistered();
        }

        [Fact]
        protected override async Task SwaggerGenerateJsonOk()
        {
            await base.Swagger_TestGenerateJson();
        }
    }
}
