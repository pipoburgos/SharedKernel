namespace SharedKernel.Application.Documents;

/// <summary> . </summary>
public interface IDocumentReaderFactory
{
    /// <summary> . </summary>
    IDocumentReader Create(string name);

    /// <summary> . </summary>
    IEnumerable<string> GetExtensions();
}