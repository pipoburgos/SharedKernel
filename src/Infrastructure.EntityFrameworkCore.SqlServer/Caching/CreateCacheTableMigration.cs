using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.Caching;

/// <summary>  </summary>
public class CreateCacheTableMigration : Migration
{
    /// <summary>  </summary>
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateCacheTable();
    }

    /// <summary>  </summary>
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropCacheTable();
    }
}
