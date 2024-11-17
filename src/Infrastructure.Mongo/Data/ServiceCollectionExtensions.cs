using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Data;
using SharedKernel.Infrastructure.Mongo.Data.DbContexts;
using SharedKernel.Infrastructure.Mongo.Data.Queries;

namespace SharedKernel.Infrastructure.Mongo.Data;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static bool IsRegistered { get; set; }

    /// <summary> Lock object to synchronize the registration of the serializer </summary>
    private static readonly object LockObject = new object();

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelMongoDbContext<TDbContext>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDbContext : MongoDbContext
    {
        if (!IsRegistered)
        {
            lock (LockObject)
            {
                if (!IsRegistered)
                {
                    BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                    IsRegistered = true;
                }
            }
        }

        return services
            .AddSharedKernelDbContext<TDbContext>(serviceLifetime)
            .AddMongoHealthChecks(configuration, "Mongo")
            .AddTransient<MongoQueryProvider>();
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelMongoUnitOfWork<TUnitOfWork, TDbContext>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TDbContext : MongoDbContext, TUnitOfWork where TUnitOfWork : class, IUnitOfWork
    {
        return services
            .AddSharedKernelMongoDbContext<TDbContext>(configuration, serviceLifetime)
            .AddScoped<TUnitOfWork>(s => s.GetRequiredService<TDbContext>());
    }
}
