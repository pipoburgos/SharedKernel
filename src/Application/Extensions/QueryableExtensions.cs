using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharedKernel.Application.Adapter;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Application.Extensions
{
    /// <summary>
    /// Queryable extensions
    /// </summary>
    public static class QueryableExtensions
    {
        #region Random

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IQueryable<T> PickRandom<T>(this IQueryable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static IQueryable<T> Shuffle<T>(this IQueryable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        #endregion Random

        #region Paging

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public static IQueryable<TResult> SatisfiedBy<T, TResult>(this IQueryable<T> queryable, ISpecification<TResult> spec) where TResult : class
        {
            return queryable
                .ProjectTo<TResult>()
                .Where(spec.SatisfiedBy());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageOptions"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderAndPaged<T>(this IQueryable<T> queryable, PageOptions pageOptions) where T : class
        {
            if (pageOptions.Orders == null || !pageOptions.Orders.Any())
                return queryable;

            return queryable
                .Order(pageOptions.Orders.ToList())
                .Skip(pageOptions.Skip)
                .Take(pageOptions.Take);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageOptions"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> queryable, PageOptions pageOptions,
            ISpecification<T> spec) where T : class
        {
            var totalBefore = queryable.Count();

            var totalAfter = queryable
                .Where(spec.SatisfiedBy())
                .Count();

            var elements = queryable
                .Where(spec.SatisfiedBy())
                .OrderAndPaged(pageOptions)
                .ToList();

            return new PagedList<T>(totalBefore, totalAfter, elements);
        }

        /// <summary>
        /// Apply IQueryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sortedColumns"></param>
        /// <returns></returns>
        private static IQueryable<T> Order<T>(this IQueryable<T> query, IList<Order> sortedColumns) where T : class
        {
            var firstColumn = sortedColumns.First();
            query = ApplyOrderBy(query, firstColumn.Ascending, firstColumn.Field.CapitalizeFirstLetter());

            foreach (var column in sortedColumns.Skip(1))
            {
                query = ApplyThenBy(query, column.Ascending, column.Field.CapitalizeFirstLetter());
            }

            return query;
        }

        /// <summary>
        ///     Convert a string to lambda expression
        ///     Example => "Person.Child.Name" : x => x.Person.Child.Name
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="ascending"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static IQueryable<TResult> ApplyOrderBy<TResult>(IQueryable<TResult> source, bool ascending,
            string propertyName) where TResult : class
        {
            var expression = new PropertyAccessorCache<TResult>().Get(propertyName);
            if (expression == null) return source;

            var order = ascending ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending);

            var resultExpression = Expression.Call(
                typeof(Queryable),
                order,
                new[] { typeof(TResult), expression.ReturnType },
                source.Expression,
                Expression.Quote(expression));

            return source.Provider.CreateQuery<TResult>(resultExpression);
        }

        /// <summary>
        ///     Convert a string to lambda expression
        ///     Example => "Person.Child.Name" : x => x.Person.Child.Name
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="ascending"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static IQueryable<TResult> ApplyThenBy<TResult>(IQueryable<TResult> source, bool ascending,
            string propertyName) where TResult : class
        {
            var expression = new PropertyAccessorCache<TResult>().Get(propertyName);
            if (expression == null) return source;

            var order = ascending ? nameof(Queryable.ThenBy) : nameof(Queryable.ThenByDescending);

            var resultExpression = Expression.Call(
                typeof(Queryable),
                order,
                new[] { typeof(TResult), expression.ReturnType },
                source.Expression,
                Expression.Quote(expression));

            return source.Provider.CreateQuery<TResult>(resultExpression);
        }

        #endregion
    }
}