using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;
using System.Text.Json;

namespace SharedKernel.Infrastructure.Elasticsearch.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelElasticsearchDbContext<TDbContext>(this IServiceCollection services, Uri uri,
        Action<JsonSerializerOptions>? configureOptions, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : ElasticsearchDbContext
    {
        return services
            .AddSharedKernelDbContext<TDbContext>(serviceLifetime)
            .AddSharedKernelElasticsearchHealthChecks(uri, configureOptions, serviceLifetime);
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelElasticsearchUnitOfWork<TUnitOfWork, TDbContext>(this IServiceCollection services,
        Uri uri, Action<JsonSerializerOptions>? configureOptions, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : ElasticsearchDbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddSharedKernelElasticsearchDbContext<TDbContext>(uri, configureOptions, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
