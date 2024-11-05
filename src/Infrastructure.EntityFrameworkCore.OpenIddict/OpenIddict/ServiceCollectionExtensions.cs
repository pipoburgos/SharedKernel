using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using SharedKernel.Application.Security;
using System.Security.Claims;
using System.Text;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.OpenIddict;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddServer<TDbContext>(this IServiceCollection services,
        IConfiguration configuration, Action<OpenIddictServerBuilder>? configure) where TDbContext : DbContext
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

        var key = Encoding.ASCII.GetBytes("clave_secreta_clave_secreta_hola");

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

                options.RegisterScopes(OpenIddictConstants.Scopes.Email, OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Roles, OpenIddictConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.OpenId, "loans");

                options.RegisterClaims(OpenIddictConstants.Claims.Role, ClaimTypes.Sid);

                // Register the encryption credentials. This sample uses a symmetric
                // encryption key that is shared between the server and the API project.
                //
                // Note: in a real world application, this encryption key should be
                // stored in a safe place (e.g in Azure KeyVault, stored as a secret).
                options.AddEncryptionKey(new SymmetricSecurityKey(key));

                options.UseAspNetCore().EnableTokenEndpointPassthrough().DisableTransportSecurityRequirement();

                options.DisableAccessTokenEncryption();

                if (configure == null)
                {
                    options.AddDevelopmentSigningCertificate();
                    //options.AddEncryptionKey(new SymmetricSecurityKey(
                    //    Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));
                }
                else
                {
                    configure.Invoke(options);
                }

            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
                options.EnableTokenEntryValidation();

                // Note: the validation handler uses OpenID Connect discovery
                // to retrieve the issuer signing keys used to validate tokens.
                options.SetIssuer(new Uri(openIdOptions.Authority!));

                options.AddAudiences(openIdOptions.Audience!);
                // Register the encryption credentials. This sample uses a symmetric
                // encryption key that is shared between the server and the API project.
                //
                // Note: in a real world application, this encryption key should be
                // stored in a safe place (e.g in Azure KeyVault, stored as a secret).
                options.AddEncryptionKey(new SymmetricSecurityKey(key));

                // Register the System.Net.Http integration.
                options.UseSystemNetHttp();
            });

        return services;
    }
}
