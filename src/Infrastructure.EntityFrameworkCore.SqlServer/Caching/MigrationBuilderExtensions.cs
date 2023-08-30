using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.Caching;

/// <summary>  </summary>
public static class MigrationBuilderExtensions
{
    /// <summary>
    /// &lt;code&gt;
    /// SET ANSI_NULLS ON
    /// GO
    /// 
    /// SET QUOTED_IDENTIFIER ON
    /// GO
    /// 
    /// CREATE TABLE [dbo].[Cache](
    ///     [Id] [nvarchar](449) NOT NULL,
    ///     [Value] [varbinary](max) NOT NULL,
    ///     [ExpiresAtTime] [datetimeoffset](7) NOT NULL,
    ///     [SlidingExpirationInSeconds] [bigint] NULL,
    ///     [AbsoluteExpiration] [datetimeoffset](7) NULL,
    /// PRIMARY KEY CLUSTERED
    /// (
    ///     [Id] ASC
    /// )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
    ///     IGNORE_DUP_KEY = OFF,
    ///     ALLOW_ROW_LOCKS = ON,
    ///     ALLOW_PAGE_LOCKS = ON,
    ///     OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    /// ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    /// GO
    /// &lt;code&gt;
    /// </summary>
    /// <param name="migrationBuilder"></param>
    /// <param name="schema"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    public static void CreateCacheTable(this MigrationBuilder migrationBuilder, string schema = "dbo",
        string table = "Cache")
    {
        migrationBuilder.Sql(@$"
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [{schema}].[{table}](
	[Id] [nvarchar](449) NOT NULL,
	[Value] [varbinary](max) NOT NULL,
	[ExpiresAtTime] [datetimeoffset](7) NOT NULL,
	[SlidingExpirationInSeconds] [bigint] NULL,
	[AbsoluteExpiration] [datetimeoffset](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
	IGNORE_DUP_KEY = OFF, 
	ALLOW_ROW_LOCKS = ON, 
	ALLOW_PAGE_LOCKS = ON, 
	OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO	
");
    }

    /// <summary>
    /// &lt;code&gt;
    /// SET ANSI_NULLS ON
    /// GO
    /// 
    /// SET QUOTED_IDENTIFIER ON
    /// GO
    /// 
    /// DROP TABLE [dbo].[Cache]
    /// &lt;code&gt;
    /// </summary>
    /// <param name="migrationBuilder"></param>
    /// <param name="schema"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    public static void DropCacheTable(this MigrationBuilder migrationBuilder, string schema = "dbo",
        string table = "Cache")
    {
        migrationBuilder.Sql(@$"
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [{schema}].[{table}]
GO	
");
    }
}
