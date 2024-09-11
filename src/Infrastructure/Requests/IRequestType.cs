namespace SharedKernel.Infrastructure.Requests;

/// <summary> . </summary>
public interface IRequestType
{
    /// <summary> . </summary>
    string UniqueName { get; }

    /// <summary> . </summary>
    Type Type { get; }

    /// <summary> . </summary>
    bool IsTopic { get; }
}
