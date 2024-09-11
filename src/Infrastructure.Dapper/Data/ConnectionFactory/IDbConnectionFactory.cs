using System.Data.Common;

namespace SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;

/// <summary> . </summary>
public interface IDbConnectionFactory
{
    /// <summary> . </summary>
    DbConnection GetConnection();
}
