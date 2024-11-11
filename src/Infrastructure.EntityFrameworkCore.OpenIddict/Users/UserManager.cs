using Microsoft.AspNetCore.Identity;
using SharedKernel.Application.Auth.Users.Services;
using System.Security.Claims;

// ReSharper disable UnusedParameter.Global

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Users;

/// <summary> . </summary>
public class UserManager : IUserManager
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    /// <summary> . </summary>
    public UserManager(UserManager<IdentityUser<Guid>> userManager)
    {
        _userManager = userManager;
    }

    /// <summary> . </summary>
    public virtual async Task CreateAsync(Guid id, string userName, string email, string password, IEnumerable<string> roles,
        IEnumerable<Claim> claims, bool emailConfirmed = false, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
            return;

        var applicationUser = new IdentityUser<Guid>
        {
            Id = id,
            Email = email,
            UserName = userName,
            EmailConfirmed = emailConfirmed,
        };

        var result = await _userManager.CreateAsync(applicationUser, password);
        Validate(result);

        result = await _userManager.AddToRolesAsync(applicationUser, roles);
        Validate(result);

        await _userManager.AddClaimsAsync(applicationUser, claims);
    }

    /// <summary> . </summary>
    public virtual async Task UpdateAsync(Guid id, string phoneNumber, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);
        applicationUser.PhoneNumber = phoneNumber;
        var result = await _userManager.UpdateAsync(applicationUser);
        Validate(result);
    }

    /// <summary> . </summary>
    public virtual async Task<List<Claim>> GetClaimsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);
        var result = await _userManager.GetClaimsAsync(applicationUser);
        return result.ToList();
    }

    /// <summary> . </summary>
    public virtual async Task AddClaimsAsync(Guid id, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);
        var result = await _userManager.AddClaimsAsync(applicationUser, claims);
        Validate(result);
    }

    /// <summary> . </summary>
    public virtual async Task AddClaimAsync(Guid id, Claim claim, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);
        var result = await _userManager.AddClaimAsync(applicationUser, claim);
        Validate(result);
    }

    /// <summary> . </summary>
    public virtual async Task RemoveClaimAsync(Guid id, Claim claim, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);
        var result = await _userManager.RemoveClaimAsync(applicationUser, claim);
        Validate(result);
    }

    /// <summary> . </summary>
    public virtual async Task AddToRolesAsync(Guid id, IEnumerable<string> roles, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);
        var result = await _userManager.AddToRolesAsync(applicationUser, roles);
        Validate(result);
    }

    /// <summary> . </summary>
    public virtual async Task<string> GenerateEmailConfirmationTokenAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);
        return await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
    }

    /// <summary> . </summary>
    public virtual async Task ConfirmEmailAsync(Guid id, string token, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);
        applicationUser.EmailConfirmed = true;
        var result = await _userManager.UpdateAsync(applicationUser);
        //var result = await _userManager.ConfirmEmailAsync(applicationUser, token);
        Validate(result);
    }

    /// <summary> . </summary>
    public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);

        var result = await _userManager.SetLockoutEnabledAsync(applicationUser, true);

        Validate(result);

        await _userManager.SetLockoutEndDateAsync(applicationUser, DateTimeOffset.MaxValue);
    }

    /// <summary> . </summary>
    public virtual async Task ReactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);

        var result = await _userManager.SetLockoutEnabledAsync(applicationUser, false);

        Validate(result);

        applicationUser.LockoutEnd = null;
        var result2 = await _userManager.UpdateAsync(applicationUser);

        Validate(result2);
        await _userManager.ResetAccessFailedCountAsync(applicationUser);
    }

    /// <summary> . </summary>
    public virtual async Task<string> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByEmailAsync(email);
        if (applicationUser == default)
            throw new Exception("Con el email que nos facilitas no existe ningún usuario");

        var confirmed = await _userManager.IsEmailConfirmedAsync(applicationUser);
        if (!confirmed)
            throw new Exception("Con el email que nos facilitas no existe ningún usuario");

        var isLocked = await _userManager.IsLockedOutAsync(applicationUser);
        if (isLocked)
            throw new Exception("Con el email que nos facilitas no existe ningún usuario");

        return await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
    }

    /// <summary> . </summary>
    public virtual async Task ResetPasswordAsync(Guid id, string token, string newPassword, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);
        var result = await _userManager.ResetPasswordAsync(applicationUser, token, newPassword);

        Validate(result);
    }

    /// <summary> . </summary>
    public virtual async Task<List<string>> GetRolesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var applicationUser = await GetUser(id);
        var roles = await _userManager.GetRolesAsync(applicationUser);
        return roles.ToList();
    }

    /// <summary> . </summary>
    private async Task<IdentityUser<Guid>> GetUser(Guid id)
    {
        var applicationUser = await _userManager.FindByIdAsync(id.ToString());

        if (applicationUser == default)
            throw new Exception($"Usuario {id} no encontrado.");

        return applicationUser;
    }

    private static void Validate(IdentityResult result)
    {
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}