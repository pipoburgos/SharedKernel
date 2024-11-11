using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Application.Auth.Applications.Commands;

/// <summary> . </summary>
public class CreateApplication : ICommandRequest
{
    /// <summary> . </summary>
    public CreateApplication(string name, string scope)
    {
        Name = name;
        Scope = scope;
    }

    /// <summary> . </summary>
    public string Name { get; }

    /// <summary> . </summary>
    public string Scope { get; }
}
