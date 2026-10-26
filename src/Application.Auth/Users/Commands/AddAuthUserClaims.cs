using SharedKernel.Application.Cqrs.Commands;
using System.Security.Claims;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed class AddAuthUserClaims : ICommandRequest
{
    /// <summary> . </summary>
    public AddAuthUserClaims(Guid id, IEnumerable<Claim> claims)
    {
        Id = id;
        Claims = claims;
    }

    /// <summary> . </summary>
    public Guid Id { get; }

    /// <summary> . </summary>
    public IEnumerable<Claim> Claims { get; }

}