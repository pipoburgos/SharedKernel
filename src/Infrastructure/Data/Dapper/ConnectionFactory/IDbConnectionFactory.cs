using System.Data.Common;

namespace SharedKernel.Infrastructure.Data.Dapper.ConnectionFactory
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DbConnection GetConnection();
    }
}
