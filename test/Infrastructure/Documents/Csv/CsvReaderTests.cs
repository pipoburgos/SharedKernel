using SharedKernel.Application.Documents;
using SharedKernel.Infrastructure.Documents.Csv;

namespace SharedKernel.Integration.Tests.Documents.Csv;

public class CsvReaderTests
{
    private readonly Stream _stream;
    private readonly ICsvReader _reader;

    public CsvReaderTests()
    {
        _stream = File.OpenRead("Documents/Csv/CsvFile.csv");
        _reader = new CsvReader();
    }

    [Fact]
    public void CastCsv()
    {
        var users = _reader
            .ReadStream(_stream)
            .Select(data => new CsvUser
            {
                Identifier = data.Get<int>(nameof(CsvUser.Identifier)),
                Username = data.Get<string>(nameof(CsvUser.Username)),
                FirstName = data.Get<string>("First name"),
                LastName = data.Get<string>("Last name")
            })
            .ToList();

        users.Count.Should().Be(5);
        users.First().Identifier.Should().Be(9012);
    }

    private class CsvUser
    {
        public int Identifier { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}