using SharedKernel.Application.Documents;
using System.Globalization;

namespace SharedKernel.Integration.Tests.Documents;

public abstract class DocumentReaderTests
{
    private readonly IDocumentReader _documentReader;
    private readonly Stream _stream;

    public DocumentReaderTests(IDocumentReader documentReader, string path)
    {
        _documentReader = documentReader;
        _stream = File.OpenRead(path);
    }

    [Fact]
    public void CastDocument()
    {
        var users = _documentReader.Configure(x => x.CultureInfo = new CultureInfo("es-ES"))
            .ReadStream(_stream)
            .Select(data => new DocumentUser
            {
                Identifier = data.Get<int>(nameof(DocumentUser.Identifier)),
                Username = data.Get<string>(nameof(DocumentUser.Username)),
                FirstName = data.Get<string?>("First name"),
                LastName = data.Get<string?>("Last name"),
                Date = data.Get<DateTime?>(4),
            })
            .ToList();

        users.Count.Should().Be(5);
        users.First().Identifier.Should().Be(9012);
        users.First().Date.Should().Be(new DateTime(2023, 12, 31));
        users.Last().LastName.Should().Be("Smith");
    }

    [Fact]
    public void CastDocumentResult()
    {
        var row = _documentReader.Configure(x => x.CultureInfo = new CultureInfo("es-ES"))
            .ReadStream(_stream)
            .Last();

        var result = row.GetResult<int>(0);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should()
            .Contain(x => x.ErrorMessage.Equals($"Cannot convert cell value to type '{nameof(Int32)}'."));


        var resultDateTime = row.GetResult<DateTime?>(4);
        resultDateTime.IsFailure.Should().BeFalse();
        resultDateTime.Value.Should().BeNull();

        var resultDateTimeNull = row.GetResult<DateTime>(4);
        resultDateTimeNull.IsFailure.Should().BeTrue();
        resultDateTimeNull.Errors.Should()
            .Contain(x => x.ErrorMessage.Equals("Type is required, cell is null"));
    }


}
