using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Infrastructure.Mongo.Data.Queries;
using SharedKernel.Infrastructure.Mongo.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Mongo.Data;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddMongoUnitOfWork<TInterface, TClass>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TClass : MongoUnitOfWorkAsync, TInterface where TInterface : IUnitOfWork
    {
        services.Add(new ServiceDescriptor(typeof(MongoUnitOfWorkAsync), typeof(MongoUnitOfWorkAsync), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TClass), typeof(TClass), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TClass), serviceLifetime));
        return services
            .AddSharedKernel()
            .AddMongoHealthChecks(configuration, "Mongo")
            .AddTransient<MongoQueryProvider>();
    }

    /// <summary>  </summary>
    public static IServiceCollection AddMongoUnitOfWorkAsync<TInterface, TClass>(this IServiceCollection services,
        IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TClass : MongoUnitOfWorkAsync, TInterface where TInterface : IUnitOfWorkAsync
    {
        services.Add(new ServiceDescriptor(typeof(TClass), typeof(TClass), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TClass), serviceLifetime));
        return services
            .AddMongoUnitOfWork<TInterface, TClass>(configuration, serviceLifetime);
    }
}
