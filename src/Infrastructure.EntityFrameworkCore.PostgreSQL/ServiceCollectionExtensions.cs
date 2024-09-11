using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    internal static IServiceCollection AddPostgreSqlHealthChecks(this IServiceCollection services,
        string connectionString, string name, params string[] tags)
    {
        var tagsList = tags.ToList();
        tagsList.Add("Postgre");
        tagsList.Add("DB");
        tagsList.Add("Sql");

        services.AddHealthChecks()
            .AddNpgSql(connectionString, "SELECT 1;", _ => { }, name, HealthStatus.Unhealthy, tagsList);

        return services;
    }
}
