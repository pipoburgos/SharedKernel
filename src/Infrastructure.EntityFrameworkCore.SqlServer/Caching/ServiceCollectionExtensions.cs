using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Caching;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Serializers;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.Caching;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// &lt;code&gt;
    /// public partial class CreateCacheTable : Migration
    /// {
    ///     protected override void Up(MigrationBuilder migrationBuilder)
    ///     {
    ///         migrationBuilder.CreateCacheTable("dbo", "Cache");
    ///     }
    /// 
    ///     protected override void Down(MigrationBuilder migrationBuilder)
    ///     {
    ///         migrationBuilder.DropCacheTable("dbo", "Cache");
    ///     }
    /// }
    /// &lt;code&gt;
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <param name="schema"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqlServerDistributedCache(this IServiceCollection services,
        string connectionString, string schema = "dbo", string table = "Cache")
    {
        return services
            .AddSqlServerHealthChecks(connectionString, "SqlServer Cache", "DistributedCache")
            .AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = connectionString;
                options.SchemaName = schema;
                options.TableName = table;
            })
            .AddTransient<IBinarySerializer, BinarySerializer>()
            .AddTransient<ICacheHelper, DistributedCacheHelper>();
    }
}

