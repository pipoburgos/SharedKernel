using FluentAssertions;
using SharedKernel.Infrastructure.Documents.Txt;
using System.IO;
using System.Linq;
using Xunit;

namespace SharedKernel.Integration.Tests.Documents.Txt
{
    public class TxtReaderTests
    {
        private readonly Stream _stream;

        public TxtReaderTests()
        {
            _stream = File.OpenRead("Documents/Txt/TxtFile.txt");
        }

        [Fact]
        public void CastTxt()
        {
            var reader = new TxtReader();
            var users = reader
                .Read(_stream, (data, _) => new CsvUser
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
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
