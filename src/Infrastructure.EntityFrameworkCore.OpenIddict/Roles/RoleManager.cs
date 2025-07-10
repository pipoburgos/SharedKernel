using Microsoft.AspNetCore.Identity;
using SharedKernel.Application.Auth.Roles.Services;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Roles;

/// <summary> . </summary>
public class RoleManager : IRoleManager
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    /// <summary> . </summary>
    public RoleManager(RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }

    /// <summary> . </summary>
    public virtual Task<bool> Exists(string role, CancellationToken cancellationToken = default)
    {
        return _roleManager.RoleExistsAsync(role);
    }

    /// <summary> . </summary>
    public virtual async Task<bool> Create(Guid id, string role, CancellationToken cancellationToken = default)
    {
        if (await _roleManager.RoleExistsAsync(role))
            return true;

        var ok = await _roleManager.CreateAsync(new IdentityRole<Guid>(role) { Id = id });
        return ok.Succeeded;
    }
}