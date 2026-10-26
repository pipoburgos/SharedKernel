using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed class RemoveAuthUser : ICommandRequest
{
    /// <summary> . </summary>
    public RemoveAuthUser(Guid id)
    {
        Id = id;
    }

    /// <summary> . </summary>
    public Guid Id { get; }
}