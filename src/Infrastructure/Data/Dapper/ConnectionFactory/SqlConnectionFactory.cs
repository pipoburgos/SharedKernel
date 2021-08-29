using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace SharedKernel.Infrastructure.Data.Dapper.ConnectionFactory
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
