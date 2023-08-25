using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace SharedKernel.Infrastructure.HttpClients;

/// <summary> Http client extensions. </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Add http client with network credentiasl.
    /// To retry with polly use AddHttpErrorPolicy included on Pipoburgos.SharedKernel.Infrastructure.Polly
    /// </summary>
    public static IHttpClientBuilder AddHttpClientNetworkCredential(this IServiceCollection services, string name,
        Uri uri, string userName, string password, string domain, Uri? uriHealthChecks = default, params string[] tags)
    {
        services.AddUriHealthChecks(uriHealthChecks ?? uri, name, tags);

        return services
            .AddHttpClient(name)
            .ConfigureHttpClientNetworkCredential(uri, userName, password, domain);
    }

    /// <summary>
    /// Add http client with bearer token.
    /// To retry with polly use AddHttpErrorPolicy included on Pipoburgos.SharedKernel.Infrastructure.Polly
    /// </summary>
    public static IHttpClientBuilder AddHttpClientBearerToken(this IServiceCollection services, string name, Uri uri)
    {
        return services
            .AddHttpClient(name)
            .ConfigureHttpClient<HttpClientAuthorizationDelegatingHandler>(services, name, uri);
    }

    /// <summary>
    /// Add http client with bearer token.
    /// To retry with polly use AddHttpErrorPolicy included on Pipoburgos.SharedKernel.Infrastructure.Polly
    /// </summary>
    public static IHttpClientBuilder AddHttpClientBearerToken<THandler>(this IServiceCollection services,
        string name, Uri uri) where THandler : DelegatingHandler
    {
        return services
            .AddHttpClient(name)
            .ConfigureHttpClient<THandler>(services, name, uri);
    }

    /// <summary>
    /// Add http client with bearer token.
    /// To retry with polly use AddHttpErrorPolicy included on Pipoburgos.SharedKernel.Infrastructure.Polly
    /// </summary>
    public static IHttpClientBuilder AddHttpClientBearerToken<TClient, THandler>(this IServiceCollection services,
        string name, Uri uri) where THandler : DelegatingHandler where TClient : class
    {
        return services
            .AddHttpClient<TClient>(name)
            .ConfigureHttpClient<THandler>(services, name, uri);
    }

    /// <summary>  </summary>
    private static IHttpClientBuilder ConfigureHttpClient<THandler>(this IHttpClientBuilder clientBuilder,
        IServiceCollection services, string name, Uri uri) where THandler : DelegatingHandler
    {
        services
            .AddTransient<THandler>()
            .AddUriHealthChecks(uri, name);

        return clientBuilder
            .ConfigureHttpClient(c => c.BaseAddress = uri)
            .AddHttpMessageHandler<THandler>();
    }

    /// <summary>  </summary>
    private static IHttpClientBuilder ConfigureHttpClientNetworkCredential(this IHttpClientBuilder clientBuilder,
        Uri uri, string userName, string password, string domain)
    {
        return clientBuilder
            .ConfigureHttpClient(c => c.BaseAddress = uri)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                Credentials = new CredentialCache
                {
                        {
                            new Uri("http://" + uri.Host), "NTLM", new NetworkCredential
                            {
                                UserName = userName,
                                Password = password,
                                Domain = domain
                            }
                        }
                }
            });
    }

    /// <summary>  </summary>
    private static void AddUriHealthChecks(this IServiceCollection services, Uri uri, string name, params string[] tags)
    {
        services
            .AddHealthChecks()
            .AddUrlGroup(uri, name, HealthStatus.Degraded, tags);
    }
}
