using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.Queryable;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Queries
{
    /// <summary>  </summary>
    public class QueryBuilder<T> where T : class
    {
        private readonly DbContext _first;
        private readonly DbContext _second;
        private readonly DbContext _third;
        private readonly PageOptions _pageOptions;
        private readonly List<ISpecification<T>> _beforeSpecifications;
        private readonly List<ISpecification<T>> _afterSpecifications;

        /// <summary>  </summary>
        public QueryBuilder(DbContext first, DbContext second, DbContext third, PageOptions pageOptions)
        {
            _first = first;
            _second = second;
            _third = third;
            _pageOptions = pageOptions;
            _beforeSpecifications = new List<ISpecification<T>>();
            _afterSpecifications = new List<ISpecification<T>>();
        }

        /// <summary>  </summary>
        public QueryBuilder<T> WhereBefore(Expression<Func<T, bool>> expr)
        {
            _beforeSpecifications.Add(new DirectSpecification<T>(expr));
            return this;
        }

        /// <summary>  </summary>
        public QueryBuilder<T> Where(Expression<Func<T, bool>> expr)
        {
            _afterSpecifications.Add(new DirectSpecification<T>(expr));
            return this;
        }

        /// <summary>  </summary>
        public QueryBuilder<T> Where(bool filter, Expression<Func<T, bool>> expr)
        {
            if (filter)
                _afterSpecifications.Add(new DirectSpecification<T>(expr));

            return this;
        }

        /// <summary>  </summary>
        public Task<IPagedList<T>> ToPagedListAsync(CancellationToken cancellationToken)
        {
            if (_third != default)
                return ToPagedListThreeContextsAsync(cancellationToken);

            if (_second != default)
                return ToPagedListTwoContextsAsync(cancellationToken);

            return ToPagedListOneContextAsync(cancellationToken);
        }

        /// <summary>  </summary>
        private async Task<IPagedList<T>> ToPagedListOneContextAsync(CancellationToken cancellationToken)
        {
            var query = _first
                .Set<T>()
                .Where(_beforeSpecifications)
                .Where(_pageOptions.ShowDeleted, _pageOptions.ShowOnlyDeleted)
                .Where(_pageOptions.FilterProperties)
                .Where(_afterSpecifications)
                .Where(_pageOptions.SearchText);

            var total = await query.CountAsync(cancellationToken);

            var elements = await query.OrderAndPaged(_pageOptions).ToListAsync(cancellationToken);

            return new PagedList<T>(total, total, elements);
        }

        /// <summary>  </summary>
        private async Task<IPagedList<T>> ToPagedListTwoContextsAsync(CancellationToken cancellationToken)
        {
            var firstQueryTask = _first
                .Set<T>()
                .Where(_beforeSpecifications)
                .Where(_pageOptions.ShowDeleted, _pageOptions.ShowOnlyDeleted)
                .Where(_pageOptions.FilterProperties)
                .Where(_afterSpecifications)
                .Where(_pageOptions.SearchText)
                .OrderAndPaged(_pageOptions)
                .ToListAsync(cancellationToken);

            var secondQueryTask = _second
                .Set<T>()
                .Where(_beforeSpecifications)
                .Where(_pageOptions.ShowDeleted, _pageOptions.ShowOnlyDeleted)
                .Where(_pageOptions.FilterProperties)
                .Where(_afterSpecifications)
                .Where(_pageOptions.SearchText)
                .CountAsync(cancellationToken);

            await Task.WhenAll(firstQueryTask, secondQueryTask);

            var firstQuery = await firstQueryTask;
            var secondQuery = await secondQueryTask;

            return new PagedList<T>(secondQuery, secondQuery, firstQuery);
        }

        /// <summary>  </summary>
        private async Task<IPagedList<T>> ToPagedListThreeContextsAsync(CancellationToken cancellationToken)
        {
            var firstQueryTask = _first
                .Set<T>()
                .Where(_beforeSpecifications)
                .Where(_pageOptions.ShowDeleted, _pageOptions.ShowOnlyDeleted)
                .Where(_pageOptions.FilterProperties)
                .Where(_afterSpecifications)
                .Where(_pageOptions.SearchText)
                .OrderAndPaged(_pageOptions)
                .ToListAsync(cancellationToken);

            var secondQueryTask = _second
                .Set<T>()
                .Where(_beforeSpecifications)
                .Where(_pageOptions.ShowDeleted, _pageOptions.ShowOnlyDeleted)
                .Where(_pageOptions.FilterProperties)
                .Where(_afterSpecifications)
                .Where(_pageOptions.SearchText)
                .CountAsync(cancellationToken);

            var thirdQueryTask = _third
                .Set<T>()
                .Where(_beforeSpecifications)
                .CountAsync(cancellationToken);

            await Task.WhenAll(firstQueryTask, secondQueryTask, thirdQueryTask);

            var firstQuery = await firstQueryTask;
            var secondQuery = await secondQueryTask;
            var thirdQuery = await thirdQueryTask;

            return new PagedList<T>(thirdQuery, secondQuery, firstQuery);
        }
    }
}
