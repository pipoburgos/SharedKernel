using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Shared.Identity;

internal sealed class CustomEmailConfirmationTokenProvider<TUser, TKey> : DataProtectorTokenProvider<TUser>
    where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
{
    public CustomEmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
        IOptions<DataProtectionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) : base(
        dataProtectionProvider, options, logger)
    {
    }
}