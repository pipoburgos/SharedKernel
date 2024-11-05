using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Identity;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelIdentity<TDbContext, TUser, TRole, TKey>(
        this IServiceCollection services, Action<IdentityOptions>? configureOptions = null)
        where TDbContext : IdentityDbContext<TUser, TRole, TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        const string mail = "MailSender";
        var mailProvider = new TokenProviderDescriptor(typeof(CustomEmailConfirmationTokenProvider<TUser, TKey>));

        services
            .AddTransient<CustomEmailConfirmationTokenProvider<TUser, TKey>>()
            .Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Tokens.ProviderMap.Add(mail, mailProvider);
                options.Tokens.EmailConfirmationTokenProvider = mail;
                options.Tokens.PasswordResetTokenProvider = mail;
                configureOptions?.Invoke(options);
            })
            .AddIdentity<TUser, TRole>()
            .AddErrorDescriber<CustomIdentityErrorDescriberSpanish>()
            .AddEntityFrameworkStores<TDbContext>();

        return services;
    }

}
