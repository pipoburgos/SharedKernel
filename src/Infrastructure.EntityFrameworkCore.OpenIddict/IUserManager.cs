using System.Security.Claims;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict;

/// <summary> . </summary>
public interface IUserManager
{
    /// <summary> . </summary>
    Task CreateAsync(Guid id, string userName, string email, string password, IEnumerable<string> roles,
        IEnumerable<Claim> claims, bool emailConfirmed = false, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task<string> GenerateEmailConfirmationTokenAsync(Guid id);

    /// <summary> . </summary>
    Task ConfirmEmailAsync(Guid id, string token);

    /// <summary> . </summary>
    Task<List<Claim>> GetClaimsAsync(Guid id);

    /// <summary> . </summary>
    Task AddClaimsAsync(Guid id, IEnumerable<Claim> claims);

    /// <summary> . </summary>
    Task AddClaimAsync(Guid id, Claim claim);

    /// <summary> . </summary>
    Task AddToRolesAsync(Guid id, IEnumerable<string> roles);

    /// <summary> . </summary>
    Task RemoveClaimAsync(Guid id, Claim claim);

    /// <summary> . </summary>
    Task RemoveAsync(Guid id);

    /// <summary> . </summary>
    Task ReactivateAsync(Guid id);

    /// <summary> . </summary>
    Task<string> GeneratePasswordResetTokenAsync(string email);

    /// <summary> . </summary>
    Task ResetPasswordAsync(Guid id, string token, string newPassword);

    /// <summary> . </summary>
    Task UpdateAsync(Guid id, string phoneNumber);

    /// <summary> . </summary>
    Task<List<string>> GetRoles(Guid id);
}