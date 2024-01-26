using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class EmailsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "parent_id",
                schema: "skr",
                table: "user",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_parent_id",
                schema: "skr",
                table: "user",
                column: "parent_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_user_parent_id",
                schema: "skr",
                table: "user",
                column: "parent_id",
                principalSchema: "skr",
                principalTable: "user",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_user_parent_id",
                schema: "skr",
                table: "user");

            migrationBuilder.DropIndex(
                name: "ix_user_parent_id",
                schema: "skr",
                table: "user");

            migrationBuilder.DropColumn(
                name: "parent_id",
                schema: "skr",
                table: "user");
        }
    }
}
