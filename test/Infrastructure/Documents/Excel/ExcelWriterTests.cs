using SharedKernel.Application.Documents;
using SharedKernel.Infrastructure.NPOI.Documents.Excel;

namespace SharedKernel.Integration.Tests.Documents.Excel;

public class ExcelWriterTests
{
    private readonly IExcelWriter _writer;

    public ExcelWriterTests()
    {
        _writer = new NpoiExcelWriter();
    }

    [Fact]
    public void CastToExcel()
    {
        var user = new ExcelUser
        {
            Identifier = 9012,
            Username = "Robert",
            FirstName = "First name",
            LastName = "Last name",
            Date = new DateOnly(2023, 12, 31)
        };

        var stream = _writer
            .Write(new List<ExcelUser> { user }, new Dictionary<string, string>
            {
                {"Identifier", "Identifier"},
                {"Username", "Username"},
                {"FirstName", "First Name"},
                {"LastName", "Last Name"},
                {"Date", "Date"}
            }, "Users");

        stream.Length.Should().BeGreaterOrEqualTo(4_150);
        stream.Length.Should().BeLessOrEqualTo(5_300);
    }

    private class ExcelUser
    {
        public int Identifier { get; set; }
        public string Username { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? Date { get; set; }
    }
}
