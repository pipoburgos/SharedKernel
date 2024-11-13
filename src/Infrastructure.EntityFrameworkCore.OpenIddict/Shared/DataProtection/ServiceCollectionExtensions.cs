using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Shared.DataProtection;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelDataProtection<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext, IDataProtectionKeyContext
    {
        services
            .Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromDays(15))
            .AddDataProtection()
            .PersistKeysToDbContext<TDbContext>();

        return services;
    }

}
