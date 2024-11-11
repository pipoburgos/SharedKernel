using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using SharedKernel.Application.Security;
using System.Security.Claims;
using System.Text;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Shared.OpenIddict;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddServer<TDbContext>(this IServiceCollection services,
        IConfiguration configuration, string encryptionKey, Action<OpenIddictServerBuilder>? configure) where TDbContext : DbContext
    {
        var openIdOptions = new OpenIdOptions();
        configuration.GetSection(nameof(OpenIdOptions)).Bind(openIdOptions);

        services.AddDbContext<TDbContext>(options =>
        {
            options.UseOpenIddict();
        });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        var key = Encoding.ASCII.GetBytes(encryptionKey);

        services.AddOpenIddict()
            .AddCore(x =>
            {
                x.UseEntityFrameworkCore().UseDbContext<TDbContext>();

            })
            .AddServer(options =>
            {
                options.AllowPasswordFlow().AllowRefreshTokenFlow().AllowClientCredentialsFlow();
                options.SetTokenEndpointUris("/connect/token");
                options.SetAccessTokenLifetime(TimeSpan.FromMinutes(10));
                options.SetIdentityTokenLifetime(TimeSpan.FromMinutes(1));
                options.SetRefreshTokenLifetime(TimeSpan.FromMinutes(30));
                options.UseAspNetCore().EnableTokenEndpointPassthrough().DisableTransportSecurityRequirement();
                options.DisableAccessTokenEncryption();

                options.RegisterScopes(OpenIddictConstants.Scopes.Email, OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Roles, OpenIddictConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.OpenId);

                options.RegisterClaims(OpenIddictConstants.Claims.Role, ClaimTypes.Sid);

                options.AddEncryptionKey(new SymmetricSecurityKey(key));

                if (configure != null)
                    configure.Invoke(options);
                else
                    options.AddDevelopmentSigningCertificate();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
                options.EnableTokenEntryValidation();
                options.SetIssuer(new Uri(openIdOptions.Authority!));
                options.AddAudiences(openIdOptions.Audience!);
                options.AddEncryptionKey(new SymmetricSecurityKey(key));
                options.UseSystemNetHttp();
            });

        return services;
    }
}
