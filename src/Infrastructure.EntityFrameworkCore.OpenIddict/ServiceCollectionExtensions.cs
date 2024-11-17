using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Auth;
using SharedKernel.Application.Auth.Applications.Services;
using SharedKernel.Application.Auth.Roles.Services;
using SharedKernel.Application.Auth.UnitOfWork;
using SharedKernel.Application.Auth.Users.Services;
using SharedKernel.Application.Security;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Applications;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Roles;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Shared;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Shared.DataProtection;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Shared.Identity;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Shared.OpenIddict;
using SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Users;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.FluentValidation;

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
            .AddHostedService<MigrationDbContext<TDbContext>>()
            .Configure<DbContextOptions<TDbContext>>(options =>
            {
                var builder = new DbContextOptionsBuilder<TDbContext>(options);
                builder.UseOpenIddict();
            })
            .Configure<OpenIdOptions>(configuration.GetSection(nameof(OpenIdOptions)))
            .AddScoped<IUserManager, UserManager>()
            .AddScoped<IRoleManager, RoleManager>()
            .AddScoped<IApplicationManager, ApplicationManager>()
            .AddSharedKernelDomainEventsSubscribers(typeof(SharedKernelApplicationAuthAssembly), typeof(SharedKernelApplicationAuthAssembly))
            .AddSharedKernelCommandsHandlers(typeof(SharedKernelApplicationAuthAssembly))
            .AddSharedKernelQueriesHandlers(typeof(SharedKernelApplicationAuthAssembly))
            .AddSharedKernelFluentValidation(typeof(SharedKernelApplicationAuthAssembly))
            .AddSharedKernelIdentity<TDbContext, TUser, TRole, Guid>(configureOptions)
            .AddScoped<IUserStore<TUser>, UserRepository<TDbContext, TUser, TRole, Guid>>()
            .AddScoped<IRoleStore<TRole>, RoleRepository<TDbContext, TUser, TRole, Guid>>()
            .AddSharedKernelDataProtection<TDbContext>()
            .AddSharedKernelServer<TDbContext>(configuration, encryptionKey, configure)
            .AddScoped<IAuthUnitOfWork>(sp => sp.GetRequiredService<TDbContext>());
    }
}
