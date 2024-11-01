using Microsoft.Data.SqlClient;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;
using System.Data.Common;

namespace SharedKernel.Infrastructure.Dapper.SqlServer.Data.ConnectionFactory;

internal class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}