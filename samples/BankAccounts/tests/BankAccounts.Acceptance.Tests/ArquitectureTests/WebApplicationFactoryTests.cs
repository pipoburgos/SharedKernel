using BankAccounts.Acceptance.Tests.Shared;
using BankAccounts.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Testing.Acceptance.Tests;

namespace BankAccounts.Acceptance.Tests.ArquitectureTests;

[Collection("Factory")]
public class WebApplicationFactoryTests : WebApplicationFactoryBaseTests<Startup>
{
    protected override Startup CreateStartup(IConfiguration configuration, WebHostBuilderContext webHostBuilderContext)
    {
        return new Startup(configuration);
    }

    protected override void ConfigureServices(Startup startup, IServiceCollection services)
    {
        startup.ConfigureServices(services);
    }

    public WebApplicationFactoryTests(BankAccountClientFactory factory) : base(factory)
    {
    }
}
