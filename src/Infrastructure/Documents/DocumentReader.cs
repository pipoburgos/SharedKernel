using SharedKernel.Application.Documents;
using System.Data;

namespace SharedKernel.Infrastructure.Documents;

/// <summary>  </summary>
public abstract class DocumentReader : IDocumentReader
{
    /// <summary>  </summary>
    public abstract string Extension { get; }

    /// <summary>  </summary>
    public DocumentReaderConfiguration Configuration { get; private set; } = new();

    /// <summary>  </summary>
    public IDocumentReader Configure(Action<DocumentReaderConfiguration> change)
    {
        Configuration = new DocumentReaderConfiguration();
        change(Configuration);
        return this;
    }

    /// <summary>  </summary>
    public abstract IEnumerable<IRowData> ReadStream(Stream stream);

    /// <summary>  </summary>
    public abstract DataTable Read(Stream stream);
}