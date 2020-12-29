using Dapper;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#if !NET461
using Microsoft.EntityFrameworkCore;
#endif

namespace SharedKernel.Infrastructure.Data.Dapper.Queries
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContextBase"></typeparam>
    public sealed class DapperQueryProvider<TDbContextBase> where TDbContextBase : DbContextBase
    {
        private readonly IDbContextFactory<TDbContextBase> _factory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        public DapperQueryProvider(IDbContextFactory<TDbContextBase> factory)
        {
            _factory = factory;
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
            using var context = _factory.CreateDbContext();
            return await context.GetConnection.QueryFirstOrDefaultAsync<T>(sql, parameters);
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
            using var context = _factory.CreateDbContext();
            return (await context.GetConnection.QueryAsync<T>(sql, parameters)).ToList();
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
            using var context = _factory.CreateDbContext();
            var count = await context.GetConnection.QueryFirstOrDefaultAsync<int>($"SELECT COUNT(1) FROM ({sql}) ALIAS", parameters);

            var queryString = sql;
            if (pageOptions.Orders != null && pageOptions.Orders.Any())
                queryString += $"{Environment.NewLine} ORDER BY {string.Join(", ", pageOptions.Orders.Select(order => order.Field + (order.Ascending ? string.Empty : " DESC")))}";

            queryString += $"{Environment.NewLine} OFFSET {pageOptions.Skip} ROWS FETCH NEXT {pageOptions.Take} ROWS ONLY";

            var items = await context.GetConnection.QueryAsync<T>(queryString, parameters);

            return new PagedList<T>(count, count, items);
        }
    }
}