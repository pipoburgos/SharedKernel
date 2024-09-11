using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Infrastructure.Polly.RetryPolicies;

namespace SharedKernel.Infrastructure.Polly.Requests.Middlewares;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddRetryPolicyMiddleware<TImp>(this IServiceCollection services,
        IConfiguration configuration) where TImp : class, IRetryPolicyExceptionHandler
    {
        return services
            .AddOptions()
            .Configure<RetrieverOptions>(configuration.GetSection(nameof(RetrieverOptions)))
            .AddTransient<IRetriever, PollyRetrieverWrap>()
            .AddTransient<IRetryPolicyExceptionHandler, TImp>()
            .AddTransient<IMiddleware, RetryPolicyMiddleware>();
    }
}
