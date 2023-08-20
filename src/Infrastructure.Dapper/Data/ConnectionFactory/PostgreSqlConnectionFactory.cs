using Npgsql;
using System.Data.Common;

namespace SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory
{
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
}
