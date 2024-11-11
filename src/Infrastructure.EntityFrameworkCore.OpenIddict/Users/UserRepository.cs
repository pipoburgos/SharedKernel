using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Users;

/// <summary> . </summary>
public class UserRepository<TDbContext, TUser, TRole, TKey> : UserStore<TUser, TRole, TDbContext, TKey>
    where TDbContext : IdentityDbContext<TUser, TRole, TKey>, IDataProtectionKeyContext
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary> . </summary>
    public UserRepository(TDbContext context) : base(context)
    {
        AutoSaveChanges = false;
    }
}