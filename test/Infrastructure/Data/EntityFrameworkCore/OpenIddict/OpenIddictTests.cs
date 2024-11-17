using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Auth.Roles.Commands;
using SharedKernel.Application.Auth.Users.Commands;
using SharedKernel.Application.Auth.Users.Services;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.OpenIddict;


public class OpenIddictTests : InfrastructureTestCase<FakeStartup>
{
    protected override string GetJsonFile()
    {
        return "Data/EntityFrameworkCore/OpenIddict/appsettings.OpenIddict.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var connection = Configuration.GetConnectionString("OpenIddictConnectionString")!;
        return services
            .AddSharedKernel()
            .AddDbContext<AuthDbContext>(o =>
                o.UseSqlServer(connection, op => op.EnableRetryOnFailure(5)).ConfigureWarnings(warnings =>
                    warnings.Log(RelationalEventId.PendingModelChangesWarning)))
            .AddSharedKernelInMemoryEventBus()
            .AddSharedKernelInMemoryCommandBus()
            .AddSharedKernelInMemoryQueryBus()
            .AddSharedKernelNewtonsoftSerializer()
            .AddSharedKernelOpenIddict<AuthDbContext, IdentityUser<Guid>, IdentityRole<Guid>>(Configuration,
                "secret_key_secret_key_secret_key");
    }

    [Fact]
    public async Task AddUser()
    {
        var commandBus = GetRequiredServiceOnNewScope<ICommandBus>();

        var id = Guid.NewGuid();

        await commandBus.Dispatch(new CreateRole(id, "testRole"), CancellationToken.None);

        var faker = new Faker();

        var userName = faker.Internet.UserName();
        var email = faker.Internet.Email();
        const string fakePassword = "PassWord$88";

        await commandBus.Dispatch(
            new CreateUser(id, userName, email, true, fakePassword, fakePassword, [], ["testRole"]),
            CancellationToken.None);


        var userManager = GetRequiredServiceOnNewScope<IUserManager>();

        var roles = await userManager.GetRolesAsync(id, CancellationToken.None);

        roles.Should().Contain("testRole");
    }
}
