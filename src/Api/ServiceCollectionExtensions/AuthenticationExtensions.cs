using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Api.Security;
using SharedKernel.Application.Security;
using System.Text;

namespace SharedKernel.Api.ServiceCollectionExtensions;

/// <summary> Authentication Extensions. </summary>
public static class AuthenticationExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelWindowsAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();

        return services
            .AddHttpContextAccessor()
            .RemoveAll<IIdentityService>()
            .AddScoped<IIdentityService, HttpContextAccessorIdentityService>()
            .AddAuthorization(options => options.FallbackPolicy = options.DefaultPolicy);
    }

    /// <summary> Configures OpenIdOptions, Authentication, cookies and bearer token. </summary>
    /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="cookieName">The cookie name. If the name is empty the cookie is not added</param>
    /// <returns></returns>
    public static IServiceCollection AddSharedKernelAuth(this IServiceCollection services,
        IConfiguration configuration, string? cookieName = default)
    {
        return services.AddSharedKernelAuth<HttpContextAccessorIdentityService>(configuration, cookieName);
    }

    /// <summary> Configures OpenIdOptions, IIdentityService, Authentication, cookies and bearer token. </summary>
    /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="cookieName">The cookie name. If the name is empty the cookie is not added</param>
    /// <returns></returns>
    public static IServiceCollection AddSharedKernelAuth<TIdentityService>(this IServiceCollection services,
    IConfiguration configuration, string? cookieName = default) where TIdentityService : class, IIdentityService
    {
        var openIdOptions = new OpenIdOptions();
        configuration.GetSection(nameof(OpenIdOptions)).Bind(openIdOptions);

        services.Configure<OpenIdOptions>(configuration.GetSection(nameof(OpenIdOptions)));

        var authenticationBuilder = services
            .AddHttpContextAccessor()
            .RemoveAll<IIdentityService>()
            .AddScoped<IIdentityService, TIdentityService>()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = openIdOptions.Authority;
                options.Audience = openIdOptions.Audience;
                options.RequireHttpsMetadata = openIdOptions.RequireHttpsMetadata;
                options.SaveToken = true;
                if (openIdOptions.Authority != default && openIdOptions.ClientSecret != default)
                {
                    var key = Encoding.ASCII.GetBytes(openIdOptions.ClientSecret);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Validate Authority
                        ValidateIssuer = true,
                        ValidIssuer = openIdOptions.Authority,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateAudience = !string.IsNullOrWhiteSpace(openIdOptions.Audience),
                        ValidAudience = openIdOptions.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                }

                // Sending the access token in the query string is required due to
                // a limitation in Browser APIs. We restrict it to only calls to the
                // SignalR hub in this code.
                // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
                // for more information about security considerations when using
                // the query string to transmit the access token.
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                };
            });

        if (!string.IsNullOrWhiteSpace(cookieName))
        {
            authenticationBuilder.AddCookie("MyCookie", options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromSeconds(openIdOptions.AccessTokenSecondsLifetime);
            });
        }


        return services;
    }
}
