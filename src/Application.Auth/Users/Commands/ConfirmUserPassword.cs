using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed class ConfirmUserPassword : ICommandRequest
{
    /// <summary> . </summary>
    public ConfirmUserPassword(Guid userId, string password, string passwordConfirmation, string token)
    {
        UserId = userId;
        Password = password;
        PasswordConfirmation = passwordConfirmation;
        Token = token;
    }

    /// <summary> . </summary>
    public Guid UserId { get; }

    /// <summary> . </summary>
    public string Password { get; }

    /// <summary> . </summary>
    public string PasswordConfirmation { get; }

    /// <summary> . </summary>
    public string Token { get; }
}