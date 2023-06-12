using System.Data;
using System.IO;

namespace SharedKernel.Application.Documents
{
    /// <summary>  </summary>
    public interface IExcelReader : IDocumentReader
    {
        /// <summary>  </summary>
        DataSet ReadTabs(Stream stream);
    }
}
