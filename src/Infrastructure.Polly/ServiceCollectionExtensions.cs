using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using SharedKernel.Infrastructure.Polly.RetryPolicies;

namespace SharedKernel.Infrastructure.Polly;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    /// <param name="httpClientBuilder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IHttpClientBuilder AddHttpErrorPolicy(this IHttpClientBuilder httpClientBuilder, IConfiguration configuration)
    {
        var retrieverOptions = GetRetrieverOptions(configuration);

        return httpClientBuilder.AddTransientHttpErrorPolicy(policyBuilder =>
            policyBuilder.WaitAndRetryAsync(retrieverOptions.RetryCount, retrieverOptions.RetryAttempt()));
    }

    /// <summary> . </summary>
    private static RetrieverOptions GetRetrieverOptions(IConfiguration configuration)
    {
        var retrieverOptions = new RetrieverOptions();
        configuration.GetSection("RetrieverOptions").Bind(retrieverOptions);
        return retrieverOptions;
    }
}
