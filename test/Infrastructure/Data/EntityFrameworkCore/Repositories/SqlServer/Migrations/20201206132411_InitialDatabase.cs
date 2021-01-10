using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedKernel.Infraestructure.Tests.Data.EntityFrameworkCore.Repositories.SqlServer.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                "skr");

            migrationBuilder.CreateTable(
                "User",
                schema: "skr",
                columns: table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    Name = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                    Emails = table.Column<string>("nvarchar(max)", nullable: true),
                    JsonAddresses = table.Column<string>("nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "User",
                "skr");
        }
    }
}
