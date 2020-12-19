using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Api
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProgramExtensions
    {
        /// <summary>
        /// Populate database and then runs the api asynchronous.
        /// Needs to call services.AddSharedKernel()
        /// Needs to register IPopulateDatabase in service collection. <see cref="IPopulateDatabase"/>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task RunAsync<TStartup>(string[] args) where TStartup : class
        {
            var host = CreateWebHostBuilder<TStartup>(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = scope.ServiceProvider.GetRequiredService<ICustomLogger>();

            try
            {
                logger.Info("Starting web host");
                var populateDatabase = services.GetRequiredService<IPopulateDatabase>();
                await populateDatabase.Populate(CancellationToken.None);
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while migrating or seeding the database.");
                logger.Fatal(ex, "Host terminated unexpectedly");
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder<TStartup>(string[] args) where TStartup : class
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<TStartup>();
        }
    }
}
