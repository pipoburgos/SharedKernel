using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Auth;
using SharedKernel.Application.Auth.Roles.Services;
using SharedKernel.Application.Auth.UnitOfWork;
using SharedKernel.Application.Auth.Users.Services;
using SharedKernel.Application.Security;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.EntityFrameworkCore.Identity.Roles;
using SharedKernel.Infrastructure.EntityFrameworkCore.Identity.Shared;
using SharedKernel.Infrastructure.EntityFrameworkCore.Identity.Shared.DataProtection;
using SharedKernel.Infrastructure.EntityFrameworkCore.Identity.Shared.Identity;
using SharedKernel.Infrastructure.EntityFrameworkCore.Identity.Users;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.FluentValidation;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Identity;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelCustomIdentity<TDbContext, TUser, TRole>(this IServiceCollection services,
        IConfiguration configuration, string encryptionKey, Action<IdentityOptions>? configureOptions = null)
        where TDbContext : IdentityDbContext<TUser, TRole, Guid>, IDataProtectionKeyContext, IAuthUnitOfWork
        where TUser : IdentityUser<Guid>, new()
        where TRole : IdentityRole<Guid>
    {
        return services
            .AddHostedService<MigrationDbContext<TDbContext>>()
            .Configure<OpenIdOptions>(configuration.GetSection(nameof(OpenIdOptions)))
            .AddScoped<IUserManager, UserManager>()
            .AddScoped<IRoleManager, RoleManager>()
            .AddSharedKernelDomainEventsSubscribers(typeof(SharedKernelApplicationAuthAssembly), typeof(SharedKernelApplicationAuthAssembly))
            .AddSharedKernelCommandsHandlers(typeof(SharedKernelApplicationAuthAssembly))
            .AddSharedKernelQueriesHandlers(typeof(SharedKernelApplicationAuthAssembly))
            .AddSharedKernelFluentValidation(typeof(SharedKernelApplicationAuthAssembly))
            .AddSharedKernelIdentity<TDbContext, TUser, TRole, Guid>(configureOptions)
            .AddScoped<IUserStore<TUser>, UserRepository<TDbContext, TUser, TRole, Guid>>()
            .AddScoped<IRoleStore<TRole>, RoleRepository<TDbContext, TUser, TRole, Guid>>()
            .AddSharedKernelDataProtection<TDbContext>();
    }
}
