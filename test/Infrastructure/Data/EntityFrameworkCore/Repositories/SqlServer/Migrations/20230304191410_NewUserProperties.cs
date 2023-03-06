using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class NewUserProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                schema: "skr",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "skr",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "skr",
                table: "User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                schema: "skr",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                schema: "skr",
                table: "User",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfChildren",
                schema: "skr",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                schema: "skr",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "skr",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "skr",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                schema: "skr",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "skr",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NumberOfChildren",
                schema: "skr",
                table: "User");
        }
    }
}
