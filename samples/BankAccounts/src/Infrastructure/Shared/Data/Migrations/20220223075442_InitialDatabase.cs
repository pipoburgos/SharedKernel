using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAccounts.Infrastructure.Shared.Data.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Birthdate = table.Column<DateTime>(type: "date", nullable: false),
                    Emails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternationalBankAccountNumber_CountryCheckDigit = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    InternationalBankAccountNumber_EntityCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    InternationalBankAccountNumber_OfficeNumber = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    InternationalBankAccountNumber_ControlDigit = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccount_User_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Movement",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Concept = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccount",
                        column: x => x.BankAccountId,
                        principalSchema: "dbo",
                        principalTable: "BankAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_OwnerId",
                schema: "dbo",
                table: "BankAccount",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Movement_BankAccountId",
                schema: "dbo",
                table: "Movement",
                column: "BankAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movement",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BankAccount",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "dbo");
        }
    }
}
