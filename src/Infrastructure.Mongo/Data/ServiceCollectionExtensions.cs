using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data;
using SharedKernel.Infrastructure.Mongo.Data.DbContexts;
using SharedKernel.Infrastructure.Mongo.Data.Queries;

namespace SharedKernel.Infrastructure.Mongo.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddMongoDbContext<TDbContext>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : MongoDbContext
    {
        return services
            .AddDbContext<TDbContext>(serviceLifetime)
            .AddMongoHealthChecks(configuration, "Mongo")
            .AddTransient<MongoQueryProvider>();
    }

    /// <summary> . </summary>
    public static IServiceCollection AddMongoUnitOfWork<TUnitOfWork, TDbContext>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : MongoDbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddMongoDbContext<TDbContext>(configuration, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
