using Microsoft.AspNetCore.Identity;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Identity;

internal sealed class RoleManager : IRoleManager
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public RoleManager(RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }

    public Task<bool> Exists(string role, CancellationToken cancellationToken)
    {
        return _roleManager.RoleExistsAsync(role);
    }

    public async Task<bool> Create(Guid id, string role, CancellationToken cancellationToken)
    {
        if (await _roleManager.RoleExistsAsync(role))
            return true;

        var ok = await _roleManager.CreateAsync(new IdentityRole<Guid>(role) { Id = id });
        return ok.Succeeded;
    }
}