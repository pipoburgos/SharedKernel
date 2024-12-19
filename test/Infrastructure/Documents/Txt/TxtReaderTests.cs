using SharedKernel.Infrastructure.Documents.Txt;

namespace SharedKernel.Integration.Tests.Documents.Txt;

public class TxtReaderTests : DocumentReaderTests
{

    public TxtReaderTests() : base(new TxtReader(), "Documents/Txt/TxtFile.txt")
    {
    }

}