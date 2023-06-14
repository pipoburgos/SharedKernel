using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SharedKernel.Testing.Acceptance.Tests
{
    public abstract class WebApplicationFactoryBaseTests<T> where T : class
    {
        protected abstract T CreateStartup(IConfiguration configuration);
        protected abstract void ConfigureServices(T startup, IServiceCollection services);

        public virtual void Startup_WhenBuildServiceCollection_ShouldDependenciesBeRegistered()
        {
            IServiceCollection serviceCollection = null;

            T startup = default;
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<T>()
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.Sources.Clear();
                            config.AddConfiguration(hostingContext.Configuration);
                            config.AddJsonFile("appsettings.json");
                            config.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Testing.json"), false);
                            startup = CreateStartup(config.Build());
                        })
                        .ConfigureServices(sc =>
                        {
                            ConfigureServices(startup, sc);
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

        public virtual async Task Swagger_TestGenerateJson()
        {
            var client = new WebApplicationFactory<T>().CreateClient();

            var response = await client.GetAsync("swagger/v1/swagger.json");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        protected abstract void StartupWhenBuildServiceCollectionShouldDependenciesBeRegistered();

        protected abstract Task SwaggerGenerateJsonOk();
    }
}
