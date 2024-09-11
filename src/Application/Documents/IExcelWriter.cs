namespace SharedKernel.Application.Documents;

/// <summary> . </summary>
public interface IExcelWriter
{
    /// <summary> . </summary>
    Stream Write<T>(IEnumerable<T> elements, Dictionary<string, string> headers, string sheetName);
}
