using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Requests.Middlewares.Failover;
using SharedKernel.Infrastructure.Requests.Middlewares.Timer;
using SharedKernel.Infrastructure.Requests.Middlewares.Validation;

namespace SharedKernel.Infrastructure.Requests.Middlewares;

/// <summary>  </summary>
public static class MiddlewaresExtensions
{
    /// <summary>  </summary>
    public static IServiceCollection AddValidationMiddleware(this IServiceCollection services)
    {
        return services
            .AddTransient<IMiddleware, ValidationMiddleware>();
    }

    /// <summary>  </summary>
    public static IServiceCollection AddTimerMiddleware<TTimeHandler>(this IServiceCollection services)
        where TTimeHandler : class, ITimeHandler
    {
        return services
            .AddTransient<ITimeHandler, TTimeHandler>()
            .AddTransient<IMiddleware, TimerMiddleware>();
    }

    /// <summary>  </summary>
    public static IServiceCollection AddTimerMiddleware(this IServiceCollection services, int milliseconds = 100)
    {
        return services
            .AddTransient<ITimeHandler>(s =>
                new TimeHandler(s.GetRequiredService<ICustomLogger<TimeHandler>>(), milliseconds))
            .AddTransient<IMiddleware, TimerMiddleware>();
    }

    /// <summary>  </summary>
    public static IServiceCollection AddFailoverMiddleware(this IServiceCollection services)
    {
        return services
            .AddTransient<FailoverCommonLogic>()
            .AddTransient<IMiddleware, FailoverMiddleware>();
    }
}
