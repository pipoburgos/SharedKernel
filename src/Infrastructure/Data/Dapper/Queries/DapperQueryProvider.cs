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
        private readonly DbConnection _connection;

        /// <summary>
        /// 
        /// </summary>
        public DapperQueryProvider(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.GetConnection();
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
            await _connection.OpenAsync();
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
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
            await _connection.OpenAsync();
            return (await _connection.QueryAsync<T>(sql, parameters)).ToList();
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
            await _connection.OpenAsync();
            var counting = _connection.QueryFirstOrDefaultAsync<int>($"SELECT COUNT(1) FROM ({sql}) ALIAS", parameters);

            var queryString = sql;
            if (pageOptions.Orders != null && pageOptions.Orders.Any())
                queryString += $"{Environment.NewLine} ORDER BY {string.Join(", ", pageOptions.Orders.Select(order => order.Field + (order.Ascending ? string.Empty : " DESC")))}";

            queryString += $"{Environment.NewLine} OFFSET {pageOptions.Skip} ROWS FETCH NEXT {pageOptions.Take} ROWS ONLY";

            var gettingItems = _connection.QueryAsync<T>(queryString, parameters);

            await Task.WhenAll(counting, gettingItems);

            return new PagedList<T>(counting.Result, counting.Result, gettingItems.Result);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection.Close();
                _connection?.Dispose();
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