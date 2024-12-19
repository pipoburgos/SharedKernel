using SharedKernel.Infrastructure.NPOI.Documents.Excel;

namespace SharedKernel.Integration.Tests.Documents.Excel;

public class ExcelReaderTests : DocumentReaderTests
{
    public ExcelReaderTests() : base(new NpoiExcelReader(),
        "Documents/Excel/ExcelFile.xlsx")
    {
    }

}
