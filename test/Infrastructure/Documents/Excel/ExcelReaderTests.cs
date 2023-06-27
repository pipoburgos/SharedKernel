using FluentAssertions;
using SharedKernel.Application.Documents;
using SharedKernel.Infrastructure.Documents.Excel.Npoi;
using System.IO;
using System.Linq;
using Xunit;

namespace SharedKernel.Integration.Tests.Documents.Excel;

public class ExcelReaderTests
{
    private readonly Stream _stream;
    private readonly IExcelReader _reader;

    public ExcelReaderTests()
    {
        _stream = File.OpenRead("Documents/Excel/ExcelFile.xlsx");
        _reader = new NpoiExcelReader();
    }

    [Fact]
    public void CastExcel()
    {
        var users = _reader
            .ReadStream(_stream)
            .Select(data => new ExcelUser
            {
                Identifier = data.Get<int>(nameof(ExcelUser.Identifier)),
                Username = data.Get<string>(nameof(ExcelUser.Username)),
                FirstName = data.Get<string>("First name"),
                LastName = data.Get<string>("Last name")
            })
            .ToList();

        users.Count.Should().Be(5);
        users.First().Identifier.Should().Be(9012);
        users.Last().LastName.Should().Be("Smith");
    }

    private class ExcelUser
    {
        public int Identifier { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
