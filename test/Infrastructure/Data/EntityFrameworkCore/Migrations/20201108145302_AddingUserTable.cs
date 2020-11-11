using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Migrations
{
    public partial class AddingUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema("skr");

            migrationBuilder.CreateTable(
                "User",
                schema: "skr",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("User", "skr");
        }
    }
}
