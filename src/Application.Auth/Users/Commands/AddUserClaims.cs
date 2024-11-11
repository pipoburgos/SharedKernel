using SharedKernel.Application.Cqrs.Commands;
using System.Security.Claims;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed class AddUserClaims : ICommandRequest
{
    /// <summary> . </summary>
    public AddUserClaims(Guid id, IEnumerable<Claim> claims)
    {
        Id = id;
        Claims = claims;
    }

    /// <summary> . </summary>
    public Guid Id { get; }

    /// <summary> . </summary>
    public IEnumerable<Claim> Claims { get; }

}