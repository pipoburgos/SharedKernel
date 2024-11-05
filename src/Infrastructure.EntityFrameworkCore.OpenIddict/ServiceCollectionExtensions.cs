using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Security;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.DataProtection;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Identity;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.OpenIddict;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddOpenIddict<TDbContext, TUser, TRole, TKey>(this IServiceCollection services,
        IConfiguration configuration, string encryptionKey, Action<IdentityOptions>? configureOptions = null,
        Action<OpenIddictServerBuilder>? configure = null)
        where TDbContext : IdentityDbContext<TUser, TRole, TKey>, IDataProtectionKeyContext
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        return services
            .Configure<OpenIdOptions>(configuration.GetSection(nameof(OpenIdOptions)))
            .AddScoped<IUserManager, UserManager>()
            .AddScoped<IRoleManager, RoleManager>()
            .AddSharedKernelIdentity<TDbContext, TUser, TRole, TKey>(configureOptions)
            .AddDataProtection<TDbContext>()
            .AddServer<TDbContext>(configuration, encryptionKey, configure);
    }
}
