using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Roles;

/// <summary> . </summary>
public class RoleRepository<TDbContext, TUser, TRole, TKey> : RoleStore<TRole, TDbContext, TKey>
    where TDbContext : IdentityDbContext<TUser, TRole, TKey>, IDataProtectionKeyContext
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary> . </summary>
    public RoleRepository(TDbContext context) : base(context)
    {
        AutoSaveChanges = false;
    }
}
