using BankAccounts.Api;
using BankAccounts.Infrastructure.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Testing.Acceptance.Authentication;
using SharedKernel.Testing.Acceptance.WebApplication;
using System.Security.Claims;

namespace BankAccounts.Acceptance.Tests.Shared
{
    public class BankAccountClientFactory : WebApplicationFactoryBase<Startup>
    {
        public override DbContext GetNewDbContext()
        {
            return Services.CreateScope().ServiceProvider.GetRequiredService<BankAccountDbContext>();
        }

        public override async Task<HttpClient> CreateClientAsync(string? language = "en-US")
        {
            var client = await base.CreateClientAsync(language);
            client.SetFakeBearerToken(GenerateClaims());
            return client;
        }

        public ClaimsIdentity GenerateClaims(string authenticationType = "Bearer")
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.Sid, "12345678-1234-1234-1234-123456789123")
            };

            var claimsIdentity = new ClaimsIdentity(claims, authenticationType, ClaimTypes.Email, ClaimTypes.Role);
            return claimsIdentity;
        }
    }
}