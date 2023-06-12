using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace SharedKernel.Application.Documents
{
    /// <summary>  </summary>
    public interface IExcelReader : IDocumentReader
    {
        /// <summary>  </summary>
        IEnumerable<T> Read<T>(Stream stream, Func<IRowData, int, T> cast, int sheetIndex);

        /// <summary>  </summary>
        DataSet ReadTabs(Stream stream, bool includeLineNumbers = true);

        /// <summary>  </summary>
        DataTable Read(Stream stream, bool includeLineNumbers, int sheetIndex);
    }
}
