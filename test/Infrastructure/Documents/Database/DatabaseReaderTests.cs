using FluentAssertions;
using SharedKernel.Application.Documents;
using SharedKernel.Infrastructure.Documents.Database.DotNetDbf;
using System.IO;
using System.Linq;
using Xunit;

namespace SharedKernel.Integration.Tests.Documents.Database;

public class DatabaseReaderTests
{
    private readonly Stream _stream;
    private readonly IDatabaseReader _reader;

    public DatabaseReaderTests()
    {
        _stream = File.OpenRead("Documents/Database/DatabaseFile.dbf");
        _reader = new DotNetDatabaseReader();
    }

    [Fact]
    public void CastDatabase()
    {
        var users = _reader
            .ReadStream(_stream)
            .Select(data => new DatabaseUser
            {
                Fid = data.Get<int>("FID"),
                Inspireid = data.Get<string>("INSPIREID"),
                Natcode = data.Get<string>("NATCODE"),
                Nameunit = data.Get<string>("NAMEUNIT"),
                Codnut1 = data.Get<string>("CODNUT1"),
                Codnut2 = data.Get<string>("CODNUT2"),
                Codnut3 = data.Get<string>("CODNUT3"),
                Codigoine = data.Get<int>("CODIGOINE"),
                ShapeLeng = data.Get<double>("SHAPE_Leng"),
                ShapeArea = data.Get<double>("SHAPE_Area")
            })
            .ToList();

        users.Count.Should().Be(8_205);
    }

    private class DatabaseUser
    {
        public int Fid { get; set; }
        public string Inspireid { get; set; }
        public string Natcode { get; set; }
        public string Nameunit { get; set; }
        public string Codnut1 { get; set; }
        public string Codnut2 { get; set; }
        public string Codnut3 { get; set; }
        public int Codigoine { get; set; }
        public double ShapeLeng { get; set; }
        public double ShapeArea { get; set; }
    }
}

