using System.Data.Common;
using Npgsql;

namespace SharedKernel.Infrastructure.Data.Dapper.ConnectionFactory
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
