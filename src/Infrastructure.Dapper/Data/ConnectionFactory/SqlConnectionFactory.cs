using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory
{
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
}
