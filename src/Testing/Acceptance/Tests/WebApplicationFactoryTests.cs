using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Testing.Acceptance.WebApplication;
using System.Net;
using Xunit;

namespace SharedKernel.Testing.Acceptance.Tests;

public abstract class WebApplicationFactoryBaseTests<T> where T : class
{
    private readonly WebApplicationFactoryBase<T> _factory;
    protected abstract T CreateStartup(IConfiguration configuration, WebHostBuilderContext webHostBuilderContext);
    protected abstract void ConfigureServices(T startup, IServiceCollection services);

    public WebApplicationFactoryBaseTests(WebApplicationFactoryBase<T> factory)
    {
        _factory = factory;
    }

    [Fact]
    public virtual void Startup_WhenBuildServiceCollection_ShouldDependenciesBeRegistered()
    {
        IServiceCollection serviceCollection = default!;

        T startup = default!;
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
                        startup = CreateStartup(config.Build(), hostingContext);
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

    [Fact]
    public virtual async Task SwaggerTestGenerateJson()
    {
        var client = await _factory.CreateClientAsync();

        var response = await client.GetAsync("swagger/v1/swagger.json");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public virtual async Task AllIsHealthy()
    {
        var client = await _factory.CreateClientAsync();

        var response = await client.GetAsync("health");

        var text = await response.Content.ReadAsStringAsync();

        text.Should().StartWith("{\"status\":\"Healthy\",");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
