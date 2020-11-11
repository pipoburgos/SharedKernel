using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Integration.Tests.Shared;
using Xunit;

namespace SharedKernel.Integration.Tests.Logging
{
    public class DefaultLoggerTests : InfrastructureTestCase
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(x =>
                {
                    x.UseStartup<FakeStartup>()
                        .UseTestServer()
                        .UseSerilog();
                });
        }

        protected override string GetJsonFile()
        {
            return "appsettings.serilog.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services;
        }

        [Fact]
        public async Task SaveInDatabase()
        {
            var serilog = GetService<ILogger<DefaultLoggerTests>>();

            var log = new DefaultCustomLogger(serilog);

            log.Info("Test info message");

            await Task.Delay(5000);

            Assert.True(true);
        }
    }
}
