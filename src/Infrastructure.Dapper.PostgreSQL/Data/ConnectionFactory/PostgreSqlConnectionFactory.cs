using Npgsql;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using System.Data.Common;

namespace SharedKernel.Infrastructure.Dapper.PostgreSQL.Data.ConnectionFactory;

internal class PostgreSqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public PostgreSqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}