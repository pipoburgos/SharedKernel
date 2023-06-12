using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace SharedKernel.Application.Documents
{
    /// <summary>  </summary>
    public interface IDocumentReader
    {
        /// <summary>  </summary>
        string Extension { get; }

        /// <summary>  </summary>
        DocumentReaderConfiguration Configuration { get; }

        /// <summary>  </summary>
        IDocumentReader Configure(Action<DocumentReaderConfiguration> change);

        /// <summary>  </summary>
        IEnumerable<T> Read<T>(Stream stream, Func<IRowData, int, T> cast);

        /// <summary>  </summary>
        DataTable Read(Stream stream);
    }
}
