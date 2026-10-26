using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using SharedKernel.Application.Auth.Applications.Services;
using SharedKernel.Application.Auth.UnitOfWork;
using SharedKernel.Application.Security;
using SharedKernel.Infrastructure.EntityFrameworkCore.Identity;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Applications;
using System.Text;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelOpenIddict<TDbContext, TUser, TRole>(this IServiceCollection services,
        IConfiguration configuration, string encryptionKey, Action<IdentityOptions>? configureOptions = null,
        Action<OpenIddictServerBuilder>? configure = null)
        where TDbContext : IdentityDbContext<TUser, TRole, Guid>, IDataProtectionKeyContext, IAuthUnitOfWork
        where TUser : IdentityUser<Guid>, new()
        where TRole : IdentityRole<Guid>
    {
        return services
            .AddSharedKernelCustomIdentity<TDbContext, TUser, TRole>(configuration, encryptionKey, configureOptions)
            .Configure<DbContextOptions<TDbContext>>(options =>
            {
                var builder = new DbContextOptionsBuilder<TDbContext>(options);
                builder.UseOpenIddict();
            })
            .AddScoped<IApplicationManager, ApplicationManager>()
            .AddSharedKernelServer<TDbContext>(configuration, encryptionKey, configure)
            .AddScoped<IAuthUnitOfWork>(sp => sp.GetRequiredService<TDbContext>());
    }

    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelServer<TDbContext>(this IServiceCollection services,
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
                options.SetAccessTokenLifetime(TimeSpan.FromSeconds(openIdOptions.AccessTokenSecondsLifetime));
                options.SetIdentityTokenLifetime(TimeSpan.FromSeconds(openIdOptions.AccessTokenSecondsLifetime));
                options.SetRefreshTokenLifetime(TimeSpan.FromSeconds(openIdOptions.RefreshTokenSecondsLifetime));
                options.UseAspNetCore().EnableTokenEndpointPassthrough().DisableTransportSecurityRequirement();
                options.DisableAccessTokenEncryption();

                options.RegisterScopes(OpenIddictConstants.Scopes.Email, OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Roles, OpenIddictConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.OpenId);

                options.RegisterClaims(OpenIddictConstants.Claims.Subject, OpenIddictConstants.Claims.Role,
                    OpenIddictConstants.Claims.PreferredUsername, OpenIddictConstants.Claims.Email);

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
