using BankAccounts.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankAccounts.Acceptance.Tests.Shared.Tests
{
    public class WebApplicationFactoryTests
    {
        [Fact]
        public void Startup_WhenBuildServiceCollection_ShouldDependenciesBeRegistered()
        {
            Startup startup = null;
            IServiceCollection serviceCollection = null;

            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.Sources.Clear();
                            config.AddConfiguration(hostingContext.Configuration);
                            config.AddJsonFile("appsettings.json");
                            config.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.testing.json"), false);
                            startup = new Startup(config.Build());
                        })
                        .ConfigureServices(sc =>
                        {
                            startup.ConfigureServices(sc);
                            serviceCollection = sc;
                        });
                })
                .Build();

            Action buildServiceProvider = () => serviceCollection.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });


            buildServiceProvider.Should().NotThrow<AggregateException>();
        }

        [Fact]
        public async Task SwaggerGenerateJsonOk()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            var response = await client.GetAsync("swagger/v1/swagger.json");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
