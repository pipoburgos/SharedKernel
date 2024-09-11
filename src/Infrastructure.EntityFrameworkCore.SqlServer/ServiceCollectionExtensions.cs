using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{

    /// <summary> . </summary>
    internal static IServiceCollection AddSqlServerHealthChecks(this IServiceCollection services,
        string connectionString, string name, params string[] tags)
    {
        var tagsList = tags.ToList();
        tagsList.Add("SqlServer");
        tagsList.Add("DB");
        tagsList.Add("Sql");

        services.AddHealthChecks()
            .AddSqlServer(connectionString, "SELECT 1;", _ => { }, name, HealthStatus.Unhealthy, tagsList);

        return services;
    }
}
