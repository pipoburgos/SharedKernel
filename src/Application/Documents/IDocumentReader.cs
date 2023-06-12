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
        IEnumerable<T> Read<T>(Stream stream, Func<IRowData, int, T> cast);

        /// <summary>  </summary>
        DataTable Read(Stream stream, bool includeLineNumbers = true);
    }
}
