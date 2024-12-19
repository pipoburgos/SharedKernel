using SharedKernel.Infrastructure.Documents.Csv;

namespace SharedKernel.Integration.Tests.Documents.Csv;

public class CsvReaderTests : DocumentReaderTests
{
    public CsvReaderTests() : base(new CsvReader(), "Documents/Csv/CsvFile.csv")
    {
    }

}