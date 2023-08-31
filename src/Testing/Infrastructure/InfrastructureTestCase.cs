using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SharedKernel.Testing.Infrastructure;

public abstract class InfrastructureTestCase<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected IConfiguration Configuration => new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile(GetJsonFile())
        .Build();

    protected override IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(x =>
            {
                x.UseStartup<TStartup>().UseTestServer();
            });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseContentRoot(Directory.GetCurrentDirectory());

        builder.ConfigureServices(services =>
        {
            ConfigureServices(services);
        });
    }

    protected virtual string GetJsonFile()
    {
        return "appsettings.json";
    }

    protected virtual IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services;
    }

    public T GetRequiredService<T>() where T : notnull
    {
        return Services.GetRequiredService<T>();
    }

    public T? GetService<T>()
    {
        return Services.GetService<T>();
    }

    public T GetRequiredServiceOnNewScope<T>() where T : notnull
    {
        return Services.CreateScope().ServiceProvider.GetRequiredService<T>();
    }

    public T? GetServiceOnNewScope<T>()
    {
        return Services.CreateScope().ServiceProvider.GetService<T>();
    }

}
