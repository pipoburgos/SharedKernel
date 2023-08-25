using BankAccounts.Acceptance.Tests.Shared;
using BankAccounts.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Testing.Acceptance.Tests;

namespace BankAccounts.Acceptance.Tests;

[Collection("Factory")]
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

    public BankAccountWebApplicationFactoryTests(BankAccountClientFactory factory) : base(factory)
    {
    }
}
