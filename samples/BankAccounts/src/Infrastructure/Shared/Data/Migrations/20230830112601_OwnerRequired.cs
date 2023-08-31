using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAccounts.Infrastructure.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class OwnerRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_User_OwnerId",
                schema: "dbo",
                table: "BankAccount");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Emails",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Concept",
                schema: "dbo",
                table: "Movement",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                schema: "dbo",
                table: "BankAccount",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InternationalBankAccountNumber_OfficeNumber",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InternationalBankAccountNumber_EntityCode",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InternationalBankAccountNumber_CountryCheckDigit",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InternationalBankAccountNumber_ControlDigit",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_User_OwnerId",
                schema: "dbo",
                table: "BankAccount",
                column: "OwnerId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_User_OwnerId",
                schema: "dbo",
                table: "BankAccount");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Emails",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Concept",
                schema: "dbo",
                table: "Movement",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                schema: "dbo",
                table: "BankAccount",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "InternationalBankAccountNumber_OfficeNumber",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<string>(
                name: "InternationalBankAccountNumber_EntityCode",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<string>(
                name: "InternationalBankAccountNumber_CountryCheckDigit",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<string>(
                name: "InternationalBankAccountNumber_ControlDigit",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_User_OwnerId",
                schema: "dbo",
                table: "BankAccount",
                column: "OwnerId",
                principalSchema: "dbo",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
