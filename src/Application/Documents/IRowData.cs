namespace SharedKernel.Application.Documents;

/// <summary> . </summary>
public interface IRowData
{
    /// <summary> . </summary>
    long LineNumber { get; }

    /// <summary> . </summary>
    T Get<T>(int index);

    /// <summary> . </summary>
    T Get<T>(string name);

    /// <summary> . </summary>
    Result<T> GetResult<T>(int index);

    /// <summary> . </summary>
    Result<T> GetResult<T>(string name);
}