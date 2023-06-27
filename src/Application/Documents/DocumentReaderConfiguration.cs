using System.Globalization;

namespace SharedKernel.Application.Documents
{
    /// <summary>  </summary>
    public class DocumentReaderConfiguration
    {
        /// <summary>  </summary>
        public bool IncludeLineNumbers { get; set; } = true;

        /// <summary>  </summary>
        public char Separator { get; set; } = ';';

        /// <summary>  </summary>
        public string ColumnLineNumberName { get; set; } = "LineNumber";

        /// <summary>  </summary>
        public int SheetIndex { get; set; } = 0;

        /// <summary>  </summary>
        public int HeadersIndex { get; set; } = 1;

        /// <summary>  </summary>
        public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;
    }
}
