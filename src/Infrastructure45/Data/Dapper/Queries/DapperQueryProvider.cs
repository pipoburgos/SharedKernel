using Dapper;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.Dapper.Queries
{
    public sealed class DapperQueryProvider<TDbContextBase> where TDbContextBase : IReadContext
    {
        private readonly Func<TDbContextBase> _factory;

        private readonly List<TDbContextBase> _contexts;

        public DapperQueryProvider(Func<TDbContextBase> factory)
        {
            _contexts = new List<TDbContextBase>();
            _factory = factory;
        }

        public Task<T> ExecuteQueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            return CreateDbContext().GetConnection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string sql, object parameters = null)
        {
            return (await CreateDbContext().GetConnection.QueryAsync<T>(sql, parameters)).ToList();
        }

        public async Task<IPagedList<T>> ToPagedListAsync<T>(string sql, object parameters, PageOptions pageOptions)
        {
            using (var connection = CreateDbContext().GetConnection)
            {
                var count = await connection.QueryFirstOrDefaultAsync<int>($"SELECT COUNT(1) FROM ({sql}) ALIAS", parameters);

                var queryString = sql;
                if (pageOptions.Orders != null && pageOptions.Orders.Any())
                    queryString += $"{Environment.NewLine} ORDER BY {string.Join(", ", pageOptions.Orders.Select(order => order.Field + (order.Ascending ? string.Empty : " DESC")))}";

                queryString += $"{Environment.NewLine} OFFSET {pageOptions.Skip} ROWS FETCH NEXT {pageOptions.Take} ROWS ONLY";

                var items = await connection.QueryAsync<T>(queryString, parameters);

                return new PagedList<T>(count, count, items);
            }
        }

        //public Task<Results<TResult>> ToResultsAsync<T, TResult>(IEnumerable<KeyValuePair<string, StringValues>> values)
        //    where T : class where TResult : class
        //{
        //    var query = CreateDbContext().Set<T>().AsNoTracking().ProjectTo<TResult>();
        //    var parser = new Parser<TResult>(values, query);
        //    return Task.FromResult(parser.Parse());
        //}

        #region Private Methods

        private TDbContextBase CreateDbContext()
        {
            var context = _factory.Invoke();

            _contexts.Add(context);

            return context;
        }

        #endregion

        private void ReleaseUnmanagedResources()
        {
            foreach (var dbContextBase in _contexts)
            {
                //dbContextBase?.GetConnection?.Dispose();
                dbContextBase?.Dispose();
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~DapperQueryProvider()
        {
            ReleaseUnmanagedResources();
        }
    }
}
