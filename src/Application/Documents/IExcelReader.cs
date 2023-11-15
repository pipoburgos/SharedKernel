using System.Data;

namespace SharedKernel.Application.Documents;

/// <summary>  </summary>
public interface IExcelReader : IDocumentReader
{
    /// <summary>  </summary>
    DataSet ReadTabs(Stream stream);
}