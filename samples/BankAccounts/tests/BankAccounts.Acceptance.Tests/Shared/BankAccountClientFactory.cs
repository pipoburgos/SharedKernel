using BankAccounts.Api;
using BankAccounts.Infrastructure.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Testing.Acceptance.Authentication;
using SharedKernel.Testing.Acceptance.WebApplication;
using System.Security.Claims;

namespace BankAccounts.Acceptance.Tests.Shared;

public class BankAccountClientFactory : WebApplicationFactoryBase<Startup>
{
    public BankAccountClientFactory()
    {
        DeleteDatabase = false;
    }

    public override DbContext GetNewDbContext()
    {
        return Services.CreateScope().ServiceProvider.GetRequiredService<BankAccountDbContext>();
    }

    public override async Task<HttpClient> CreateClientAsync(string? language = "en-US")
    {
        var client = await base.CreateClientAsync(language);
        client.AddBearerToken(GenerateClaims());
        return client;
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.SetFakeJwtBearerHandler();
        //services.RemoveAll<IDateTime>().AddTransient(_ =>
        //{
        //    var dateTime = Substitute.For<IDateTime>();
        //    dateTime.UtcNow.Returns(DateTime ?? System.DateTime.UtcNow);
        //    return dateTime;
        //});
    }

    private static List<Claim> GenerateClaims()
    {
        return [new(ClaimTypes.Sid, "12345678-1234-1234-1234-123456789123")];
    }
}
