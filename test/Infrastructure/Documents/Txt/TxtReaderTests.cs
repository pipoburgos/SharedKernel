using SharedKernel.Application.Documents;
using SharedKernel.Infrastructure.Documents.Txt;

namespace SharedKernel.Integration.Tests.Documents.Txt;

public class TxtReaderTests
{
    private readonly Stream _stream;
    private readonly ITxtReader _reader;

    public TxtReaderTests()
    {
        _stream = File.OpenRead("Documents/Txt/TxtFile.txt");
        _reader = new TxtReader();
    }

    [Fact]
    public void CastTxt()
    {
        var users = _reader
            .ReadStream(_stream)
            .Select(data => new TxtUser
            {
                Identifier = data.Get<int>(nameof(TxtUser.Identifier)),
                Username = data.Get<string>(nameof(TxtUser.Username)),
                FirstName = data.Get<string>("First name"),
                LastName = data.Get<string>("Last name")
            })
            .ToList();

        users.Count.Should().Be(5);
        users.First().Identifier.Should().Be(9012);
    }

    private class TxtUser
    {
        public int Identifier { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}