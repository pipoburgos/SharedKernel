using SharedKernel.Application.Cqrs.Commands;
using System.Security.Claims;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed class CreateUser : ICommandRequest
{
    /// <summary> . </summary>
    public CreateUser(Guid id, string userName, string email, bool emailConfirmed, string password,
        string passwordConfirmation, IEnumerable<Claim> claims, IEnumerable<string> roles)
    {
        Id = id;
        UserName = userName;
        Email = email;
        EmailConfirmed = emailConfirmed;
        Password = password;
        PasswordConfirmation = passwordConfirmation;
        Claims = claims;
        Roles = roles;
    }

    /// <summary> . </summary>
    public Guid Id { get; }

    /// <summary> . </summary>
    public string UserName { get; }

    /// <summary> . </summary>
    public string Email { get; }

    /// <summary> . </summary>
    public bool EmailConfirmed { get; }

    /// <summary> . </summary>
    public string Password { get; }

    /// <summary> . </summary>
    public string PasswordConfirmation { get; }

    /// <summary> . </summary>
    public IEnumerable<Claim> Claims { get; }

    /// <summary> . </summary>
    public IEnumerable<string> Roles { get; }
}