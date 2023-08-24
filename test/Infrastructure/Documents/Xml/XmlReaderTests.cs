using FluentAssertions;
using SharedKernel.Application.Documents;
using SharedKernel.Infrastructure.Documents.Xml;
using Xunit;

namespace SharedKernel.Integration.Tests.Documents.Xml;

public class XmlReaderTests
{
    private readonly Stream _stream;
    private readonly IXmlReader _reader;

    public XmlReaderTests()
    {
        _stream = File.OpenRead("Documents/Xml/XmlFile.xml");
        _reader = new XmlReader();
    }

    [Fact]
    public void CastXml()
    {
        var books = _reader
            .ReadStream(_stream)
            .Select(data => new Book
            {
                Id = data.Get<string>("id"),
                Author = data.Get<string>("author"),
                Price = data.Get<double>("price"),
                PublishDate = data.Get<DateTime>("publish_date")
            })
            .ToList();

        books.Count.Should().Be(12);
        books.First().Id.Should().Be("bk101");
        books.First().Price.Should().Be(44.95);
        books.First().PublishDate.Should().Be(new DateTime(2000, 10, 1));
        books.Last().Id.Should().Be("bk112");
    }

    private class Book
    {
        public string Id { get; set; }

        public string Author { get; set; }

        public double Price { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
