using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed class RemoveUser : ICommandRequest
{
    /// <summary> . </summary>
    public RemoveUser(Guid id)
    {
        Id = id;
    }

    /// <summary> . </summary>
    public Guid Id { get; }
}