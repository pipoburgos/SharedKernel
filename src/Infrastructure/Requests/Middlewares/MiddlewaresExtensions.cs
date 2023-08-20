using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Requests.Middlewares.Timer;

namespace SharedKernel.Infrastructure.Requests.Middlewares;

/// <summary>  </summary>
public static class MiddlewaresExtensions
{




    /// <summary>  </summary>
    public static IServiceCollection AddTimerMiddleware<TTimeHandler>(this IServiceCollection services) where TTimeHandler : class, ITimeHandler
    {
        return services
            .AddTransient<ITimeHandler, TTimeHandler>()
            .AddTransient(typeof(IMiddleware<>), typeof(TimerMiddleware<>))
            .AddTransient(typeof(IMiddleware<,>), typeof(TimerMiddleware<,>));
    }

    /// <summary>  </summary>
    public static IServiceCollection AddTimerMiddleware(this IServiceCollection services, int milliseconds = 100)
    {
        return services
            .AddTransient<ITimeHandler>(s => new TimeHandler(s.GetRequiredService<ICustomLogger<TimeHandler>>(), milliseconds))
            .AddTransient(typeof(IMiddleware<>), typeof(TimerMiddleware<>))
            .AddTransient(typeof(IMiddleware<,>), typeof(TimerMiddleware<,>));
    }
}
