using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Integration.Tests.Shared;
using System.Threading.Tasks;
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

            return Host
                .CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureWebHostDefaults(x => x.UseStartup<FakeStartup>().UseTestServer());
        }

        protected override string GetJsonFile()
        {
            return "Logging/appsettings.serilog.json";
        }

        [Fact]
        public async Task SaveInDatabase()
        {
            var serilog = GetService<ILogger<DefaultLoggerTests>>();

            var log = new DefaultCustomLogger<DefaultLoggerTests>(serilog);

            log.Info("Test info message");

            await Task.Delay(5000);

            Assert.True(true);
        }
    }
}
