using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace SharedKernel.Integration.Tests.Shared
{
    public abstract class InfrastructureTestCase : WebApplicationFactory<FakeStartup>
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
                    x.UseStartup<FakeStartup>()
                        .UseTestServer();
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

        protected T GetService<T>()
        {
            return Services.GetService<T>();
        }

    }
}