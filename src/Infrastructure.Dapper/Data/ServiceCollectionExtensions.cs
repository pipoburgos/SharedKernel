using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using SharedKernel.Infrastructure.Dapper.Data.Queries;
using System.Collections;

namespace SharedKernel.Infrastructure.Dapper.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddDapperSqlServer(this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.AddHealthChecks()
            .AddSqlServer(connectionString, "SELECT 1;", _ => { }, "Sql Server Dapper",
                HealthStatus.Unhealthy, new[] { "DB", "Sql", "SqlServer" });

        services.Add(new ServiceDescriptor(typeof(DapperQueryProvider), typeof(DapperQueryProvider), serviceLifetime));
        return services
            .AddTransient<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
    }

    /// <summary>  </summary>
    public static IServiceCollection AddDapperPostgreSql(this IServiceCollection services, string connectionString,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.AddHealthChecks()
            .AddNpgSql(connectionString, "SELECT 1;", null, "Postgre Dapper");

        services.Add(new ServiceDescriptor(typeof(DapperQueryProvider), typeof(DapperQueryProvider), serviceLifetime));

        return services
            .AddTransient<IDbConnectionFactory>(_ => new PostgreSqlConnectionFactory(connectionString));
    }

    /// <summary>  </summary>
    public static string ShowDapperQuery(this string sql, DynamicParameters? parameters = null)
    {
        var result = sql;

        if (parameters == null || !parameters.ParameterNames.Any())
            return result;

        foreach (var param in parameters.ParameterNames.Reverse())
        {
            var value = parameters.Get<object>(param);
            var parametro = $"@{param}";

            if (value is Enum enumerado)
            {
                result = result.Replace(parametro, $"{enumerado.ToString("D")}");
            }
            else if (value.GetType() != typeof(string) && value is IEnumerable list)
            {
                result = result.Replace(parametro, $"({string.Join(",", list.Cast<object>().Select(x => x.ToString()))})");
            }
            else if (value is DateTime fecha)
            {
                result = result.Replace(parametro, $"'{fecha:yyyyMMdd}'");
            }
            else
            {
                result = result.Replace(parametro, $"'{value}'");
            }
        }

        return result;
    }

    /// <summary>  </summary>
    public static string ShowDapperParameters(this string sql, DynamicParameters parameters)
    {
        var parametersLines = new List<string>();
        var result = sql;

        foreach (var param in parameters.ParameterNames.Reverse())
        {
            var value = parameters.Get<object>(param);
            var parametro = $"DECLARE @{param} AS ";

            if (value is Enum enumerado)
            {
                parametro += $"INT = {enumerado.ToString("D")}";
            }
            else if (value is int entero)
            {
                parametro += $"INT = {entero}";
            }
            else if (value is Guid guid)
            {
                parametro += $"UNIQUEIDENTIFIER = '{guid}'";
            }
            else if (value.GetType() != typeof(string) && value is IEnumerable list)
            {
                result = result.Replace(parametro, $"({string.Join(",", list.Cast<object>().Select(x => x.ToString()))})");
            }
            else if (value is DateTime fecha)
            {
                parametro += $"DATE = '{fecha:yyyyMMdd}'";
            }
            else if (value is string text)
            {
                parametro += $"NVARCHAR({text.Length}) = '{text}'";
            }
            parametersLines.Add(parametro);
        }

        var result2 = $"{string.Join("\n", parametersLines)}\n\n\n{result}";
        return result2;
    }
}
