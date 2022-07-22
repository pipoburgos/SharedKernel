using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Logging;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Infrastructure.RetryPolicies;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares
{
    /// <summary>  </summary>
    public static class MiddlewaresExtensions
    {
        /// <summary>  </summary>
        public static IServiceCollection AddValidationMiddleware(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IMiddleware<>), typeof(ValidationMiddleware<>))
                .AddTransient(typeof(IMiddleware<,>), typeof(ValidationMiddleware<,>));
        }

        /// <summary>  </summary>
        public static IServiceCollection AddRetryPolicyMiddleware<TImp>(this IServiceCollection services) where TImp : class, IRetryPolicyExceptionHandler
        {
            return services
                .AddTransient<IRetriever, PollyRetriever>()
                .AddTransient<IRetryPolicyExceptionHandler, TImp>()
                .AddTransient(typeof(IMiddleware<>), typeof(RetryPolicyMiddleware<>))
                .AddTransient(typeof(IMiddleware<,>), typeof(RetryPolicyMiddleware<,>));
        }

        /// <summary>  </summary>
        public static IServiceCollection AddTimerMiddleware(this IServiceCollection services, int milliseconds = 50)
        {
            return services
                .AddTransient<ITimeHandler>(s => new TimeHandler(s.GetRequiredService<ICustomLogger<TimeHandler>>(), milliseconds))
                .AddTransient(typeof(IMiddleware<>), typeof(TimerMiddleware<>))
                .AddTransient(typeof(IMiddleware<,>), typeof(TimerMiddleware<,>));
        }
    }
}
