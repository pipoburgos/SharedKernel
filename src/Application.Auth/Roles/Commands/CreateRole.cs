using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Application.Auth.Roles.Commands;

/// <summary> . </summary>
public sealed class CreateRole : ICommandRequest
{
    /// <summary> . </summary>
    public CreateRole(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary> . </summary>
    public Guid Id { get; }

    /// <summary> . </summary>
    public string Name { get; }
}