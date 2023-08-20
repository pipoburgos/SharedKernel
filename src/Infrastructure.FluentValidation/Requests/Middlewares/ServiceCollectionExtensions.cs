using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;

namespace SharedKernel.Infrastructure.FluentValidation.Requests.Middlewares;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddValidationMiddleware(this IServiceCollection services)
    {
        return services
            .AddTransient(typeof(IMiddleware<>), typeof(ValidationMiddleware<>))
            .AddTransient(typeof(IMiddleware<,>), typeof(ValidationMiddleware<,>));
    }
}
