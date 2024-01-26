using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ParentToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                schema: "skr",
                table: "User",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ParentId",
                schema: "skr",
                table: "User",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_ParentId",
                schema: "skr",
                table: "User",
                column: "ParentId",
                principalSchema: "skr",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_ParentId",
                schema: "skr",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ParentId",
                schema: "skr",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "skr",
                table: "User");
        }
    }
}
