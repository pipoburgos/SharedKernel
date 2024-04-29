using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace BankAccounts.Api;

/// <summary> Program. </summary>
public sealed class Program
{
    /// <summary> Main. </summary>
    /// <param name="args"></param>
    [ExcludeFromCodeCoverage]
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration().CreateLogger();

        try
        {
            await CreateHostBuilder(args).Build().RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex.Message);
            return 1;
        }
    }

    /// <summary> CreateHostBuilder. </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((hostingContext, loggerConfiguration) =>
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}