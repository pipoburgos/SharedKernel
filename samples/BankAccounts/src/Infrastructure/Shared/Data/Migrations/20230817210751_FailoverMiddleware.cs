using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAccounts.Infrastructure.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class FailoverMiddleware : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorRequest",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OccurredOn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorRequest", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorRequest",
                schema: "dbo");
        }
    }
}
