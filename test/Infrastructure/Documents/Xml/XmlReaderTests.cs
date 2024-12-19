using SharedKernel.Infrastructure.Documents.Xml;

namespace SharedKernel.Integration.Tests.Documents.Xml;

public class XmlReaderTests : DocumentReaderTests
{
    public XmlReaderTests() : base(new XmlReader(), "Documents/Xml/XmlFile.xml")
    {
    }

}
