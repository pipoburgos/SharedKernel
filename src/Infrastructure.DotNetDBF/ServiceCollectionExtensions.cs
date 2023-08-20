using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Documents;
using SharedKernel.Infrastructure.DotNetDBF.Documents.Database;

namespace SharedKernel.Infrastructure.DotNetDBF;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddDotNetDatabaseReader(this IServiceCollection services)
    {
        return services
            .AddTransient<IDatabaseReader, DotNetDatabaseReader>();
    }
}
