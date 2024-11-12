#if NET8_0_OR_GREATER
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    private async Task Migrate()
    {
        var dbContext = GetRequiredServiceOnNewScope<AuthDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var connection = Configuration.GetConnectionString("AuthConnectionString")!;
        return services
            .AddSharedKernel()
            .AddDbContext<AuthDbContext>(o => o.UseSqlServer(connection))
            .AddInMemoryEventBus()
            .AddInMemoryCommandBus()
            .AddInMemoryQueryBus()
            .AddNewtonsoftSerializer()
            .AddSharedKernelOpenIddict<AuthDbContext, IdentityUser<Guid>, IdentityRole<Guid>>(Configuration,
                "secret_key_secret_key_secret_key");
    }

    [Fact]
    public async Task AddUser()
    {
        var commandBus = GetRequiredServiceOnNewScope<ICommandBus>();

        await Migrate();

        var id = Guid.NewGuid();

        await commandBus.Dispatch(new CreateRole(id, "testRole"), CancellationToken.None);

        await commandBus.Dispatch(
            new CreateUser(id, "Roberto", "a@a.es", true, "PassWord$88", "PassWord$", [], ["testRole"]),
            CancellationToken.None);


        var userManager = GetRequiredServiceOnNewScope<IUserManager>();

        var roles = await userManager.GetRolesAsync(id, CancellationToken.None);

        roles.Should().Contain("testRole");
    }
}
#endif