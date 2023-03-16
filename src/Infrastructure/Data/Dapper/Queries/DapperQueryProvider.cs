using Dapper;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Infrastructure.Data.Dapper.ConnectionFactory;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.Dapper.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DapperQueryProvider : IDisposable
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly List<DbConnection> _connections;

        /// <summary>
        /// 
        /// </summary>
        public DapperQueryProvider(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _connections = new List<DbConnection>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<T> ExecuteQueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            var connection = _dbConnectionFactory.GetConnection();
            _connections.Add(connection);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> ExecuteQueryAsync<T>(string sql, object parameters = null)
        {
            var connection = _dbConnectionFactory.GetConnection();
            _connections.Add(connection);
            await connection.OpenAsync();
            return (await connection.QueryAsync<T>(sql, parameters)).ToList();
        }

        /// <summary>  </summary>
        public QueryBuilder Set(PageOptions pageOptions)
        {
            var connection = _dbConnectionFactory.GetConnection();
            _connections.Add(connection);
            return new QueryBuilder(connection.ConnectionString, pageOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="pageOptions"></param>
        /// <returns></returns>
        public async Task<IPagedList<T>> ToPagedListAsync<T>(string sql, object parameters, PageOptions pageOptions)
        {
            var connection = _dbConnectionFactory.GetConnection();
            _connections.Add(connection);
            await connection.OpenAsync();

            var total = await connection.QueryFirstOrDefaultAsync<int>($"SELECT COUNT(1) FROM ({sql}) ALIAS", parameters);

            if (total == default)
                return PagedList<T>.Empty();

            var queryString = sql;
            if (pageOptions.Orders != null && pageOptions.Orders.Any())
                queryString += $"{Environment.NewLine} ORDER BY {string.Join(", ", pageOptions.Orders.Select(order => order.Field + (order.Ascending ? string.Empty : " DESC")))}";

            if (pageOptions.Take.HasValue)
                queryString += $"{Environment.NewLine} OFFSET {pageOptions.Skip} ROWS FETCH NEXT {pageOptions.Take} ROWS ONLY";

            var elements = await connection.QueryAsync<T>(queryString, parameters);

            return new PagedList<T>(total, elements);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            foreach (var dbConnection in _connections)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        ~DapperQueryProvider()
        {
            Dispose(false);
        }
    }
}