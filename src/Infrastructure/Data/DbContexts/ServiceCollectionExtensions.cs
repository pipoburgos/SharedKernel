using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Validator;

namespace SharedKernel.Infrastructure.Data.DbContexts;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        return services
            .AddSharedKernel()
            .AddTransient<IEntityAuditableService, EntityAuditableService>()
            .AddTransient<IClassValidatorService, ClassValidatorService>();
    }
}
