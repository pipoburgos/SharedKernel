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
    public class QueryBuilderDto<T, TResult> where T : class
    {
        private readonly DbContext _first;
        private readonly DbContext _second;
        private readonly DbContext _third;
        private readonly PageOptions _pageOptions;
        private readonly List<ISpecification<T>> _beforeSpecifications;
        private readonly List<ISpecification<T>> _afterSpecifications;
        private readonly List<ISpecification<TResult>> _dtoSpecifications;
        private Expression<Func<T, TResult>> _selector;

        /// <summary>  </summary>
        public QueryBuilderDto(DbContext first, DbContext second, DbContext third, PageOptions pageOptions)
        {
            _first = first;
            _second = second;
            _third = third;
            _pageOptions = pageOptions;
            _beforeSpecifications = new List<ISpecification<T>>();
            _afterSpecifications = new List<ISpecification<T>>();
            _dtoSpecifications = new List<ISpecification<TResult>>();
        }

        /// <summary>  </summary>
        public QueryBuilderDto<T, TResult> WhereBefore(Expression<Func<T, bool>> expr)
        {
            _beforeSpecifications.Add(new DirectSpecification<T>(expr));
            return this;
        }

        /// <summary>  </summary>
        public QueryBuilderDto<T, TResult> Where(Expression<Func<T, bool>> expr)
        {
            _afterSpecifications.Add(new DirectSpecification<T>(expr));
            return this;
        }

        /// <summary>  </summary>
        public QueryBuilderDto<T, TResult> Where(bool filter, Expression<Func<T, bool>> expr)
        {
            if (filter)
                _afterSpecifications.Add(new DirectSpecification<T>(expr));

            return this;
        }

        /// <summary>  </summary>
        public QueryBuilderDto<T, TResult> WhereDto(Expression<Func<TResult, bool>> expr)
        {
            _dtoSpecifications.Add(new DirectSpecification<TResult>(expr));
            return this;
        }

        /// <summary>  </summary>
        public QueryBuilderDto<T, TResult> Select(Expression<Func<T, TResult>> select)
        {
            _selector = select;
            return this;
        }

        /// <summary>  </summary>
        public Task<IPagedList<TResult>> ToPagedListAsync(CancellationToken cancellationToken)
        {
            if (_third != default)
                return ToPagedListThreeContextsAsync(cancellationToken);

            if (_second != default)
                return ToPagedListTwoContextsAsync(cancellationToken);

            return ToPagedListOneContextAsync(cancellationToken);
        }

        /// <summary>  </summary>
        private async Task<IPagedList<TResult>> ToPagedListOneContextAsync(CancellationToken cancellationToken)
        {
            var query = _first
                .Set<T>()
                .AsNoTracking()
                .Where(_beforeSpecifications)
                .Where(_pageOptions.ShowDeleted, _pageOptions.ShowOnlyDeleted)
                .Where(_pageOptions.FilterProperties)
                .Where(_afterSpecifications)
                .MapToDto(_selector)
                .Where(_dtoSpecifications)
                .Where(_pageOptions.SearchText);

            var total = await query.CountAsync(cancellationToken);

            var elements = await query.OrderAndPaged(_pageOptions).ToListAsync(cancellationToken);

            return new PagedList<TResult>(total, total, elements);
        }

        /// <summary>  </summary>
        private async Task<IPagedList<TResult>> ToPagedListTwoContextsAsync(CancellationToken cancellationToken)
        {
            var firstQueryTask = _first
                .Set<T>()
                .AsNoTracking()
                .Where(_beforeSpecifications)
                .Where(_pageOptions.ShowDeleted, _pageOptions.ShowOnlyDeleted)
                .Where(_pageOptions.FilterProperties)
                .Where(_afterSpecifications)
                .MapToDto(_selector)
                .Where(_dtoSpecifications)
                .Where(_pageOptions.SearchText)
                .OrderAndPaged(_pageOptions)
                .ToListAsync(cancellationToken);

            var secondQueryTask = _second
                .Set<T>()
                .AsNoTracking()
                .Where(_beforeSpecifications)
                .Where(_pageOptions.ShowDeleted, _pageOptions.ShowOnlyDeleted)
                .Where(_pageOptions.FilterProperties)
                .Where(_afterSpecifications)
                .MapToDto(_selector)
                .Where(_dtoSpecifications)
                .Where(_pageOptions.SearchText)
                .CountAsync(cancellationToken);


            await Task.WhenAll(firstQueryTask, secondQueryTask);

            var firstQuery = await firstQueryTask;
            var secondQuery = await secondQueryTask;

            return new PagedList<TResult>(secondQuery, secondQuery, firstQuery);
        }

        /// <summary>  </summary>
        private async Task<IPagedList<TResult>> ToPagedListThreeContextsAsync(CancellationToken cancellationToken)
        {
            var firstQueryTask = _first
                .Set<T>()
                .AsNoTracking()
                .Where(_beforeSpecifications)
                .Where(_pageOptions.ShowDeleted, _pageOptions.ShowOnlyDeleted)
                .Where(_pageOptions.FilterProperties)
                .Where(_afterSpecifications)
                .MapToDto(_selector)
                .Where(_dtoSpecifications)
                .Where(_pageOptions.SearchText)
                .OrderAndPaged(_pageOptions)
                .ToListAsync(cancellationToken);

            var secondQueryTask = _second
                .Set<T>()
                .AsNoTracking()
                .Where(_beforeSpecifications)
                .Where(_pageOptions.ShowDeleted, _pageOptions.ShowOnlyDeleted)
                .Where(_pageOptions.FilterProperties)
                .Where(_afterSpecifications)
                .MapToDto(_selector)
                .Where(_dtoSpecifications)
                .Where(_pageOptions.SearchText)
                .CountAsync(cancellationToken);

            var thirdQueryTask = _third
                .Set<T>()
                .AsNoTracking()
                .Where(_beforeSpecifications)
                .CountAsync(cancellationToken);

            await Task.WhenAll(firstQueryTask, secondQueryTask, thirdQueryTask);

            var firstQuery = await firstQueryTask;
            var secondQuery = await secondQueryTask;
            var thirdQuery = await thirdQueryTask;

            return new PagedList<TResult>(thirdQuery, secondQuery, firstQuery);
        }
    }
}
