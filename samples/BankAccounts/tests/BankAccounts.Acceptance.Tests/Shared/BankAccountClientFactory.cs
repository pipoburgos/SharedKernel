using BankAccounts.Api;
using BankAccounts.Infrastructure.Shared.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedKernel.Application.Events;
using SharedKernel.Infrastructure.Events.Synchronous;

namespace BankAccounts.Acceptance.Tests.Shared
{
    public class BankAccountClientFactory : WebApplicationFactory<Startup>
    {
        private bool _firstTime = true;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder
                .ConfigureAppConfiguration((_, conf) =>
                {
                    conf.AddJsonFile("appsettings.json");
                    conf.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.testing.json"), false);
                })
                .ConfigureServices(_ =>
                {
                    //s.RemoveAll<IEmailSender>().AddTransient(_ => Substitute.For<IEmailSender>());
                })
                // Llama al método Startup.ConfigureServices
                .ConfigureTestServices(services =>
                {
                    services
                        .RemoveAll<IEventBus>()
                        .AddSingleton<IEventBus, SynchronousEventBus>();
                });
        }

        public async Task<HttpClient> CreateClientAsync()
        {
            if (!_firstTime)
                return CreateClient();

            await RegenerateDatabase(CancellationToken.None);
            _firstTime = false;
            return CreateClient();
        }

        private async Task RegenerateDatabase(CancellationToken cancellationToken)
        {
            await Task.Delay(15_000, cancellationToken);
            var unitOfWork = Services.CreateScope().ServiceProvider.GetRequiredService<BankAccountDbContext>();
            await unitOfWork.Database.EnsureDeletedAsync(cancellationToken);
            unitOfWork.Database.SetCommandTimeout(300);
            await unitOfWork.Database.MigrateAsync(cancellationToken);
        }
    }
}