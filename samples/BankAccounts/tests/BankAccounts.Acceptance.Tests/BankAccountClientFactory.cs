using BankAccounts.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using SharedKernel.Application.Communication.Email;
using System.IO;

namespace BankAccounts.Acceptance.Tests
{
    public class BankAccountClientFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder
                .ConfigureAppConfiguration((_, conf) =>
                {
                    conf.AddJsonFile("appsettings.json");
                    conf.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.testing.json"), false);
                })
                .ConfigureServices(s =>
                {
                    s.AddTransient(_ => Substitute.For<IEmailSender>());
                })
                // Llama al método Startup.ConfigureServices
                .ConfigureTestServices(_ =>
                {
                    //s.RemoveAll<IEmailSender>();
                });
        }
    }
}
