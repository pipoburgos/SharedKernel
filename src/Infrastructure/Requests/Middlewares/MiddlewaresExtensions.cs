using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.Repositories;
using SharedKernel.Infrastructure.Requests.Middlewares.Failover;
using SharedKernel.Infrastructure.Requests.Middlewares.Timer;
using SharedKernel.Infrastructure.Requests.Middlewares.Validation;

namespace SharedKernel.Infrastructure.Requests.Middlewares;

/// <summary>  </summary>
public static class MiddlewaresExtensions
{
    /// <summary> IMPORTANT!!! Add modelBuilder.ApplyConfiguration(new ErrorRequestConfiguration()) </summary>
    public static IServiceCollection AddEntityFrameworkFailoverMiddleware<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        return services
            .AddTransient<IRequestFailoverRepository, EntityFrameworkCoreRequestFailoverRepository<TContext>>()
            .AddTransient<FailoverCommonLogic>()
            .AddTransient(typeof(IMiddleware<>), typeof(FailoverMiddleware<>))
            .AddTransient(typeof(IMiddleware<,>), typeof(FailoverMiddleware<,>));
    }

    /// <summary>  </summary>
    public static IServiceCollection AddValidationMiddleware(this IServiceCollection services)
    {
        return services
            .AddTransient(typeof(IMiddleware<>), typeof(ValidationMiddleware<>))
            .AddTransient(typeof(IMiddleware<,>), typeof(ValidationMiddleware<,>));
    }



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
