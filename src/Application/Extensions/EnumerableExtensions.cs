using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Application.Exceptions;
using SharedKernel.Domain.Specifications.Common;
using System.Linq.Expressions;

namespace SharedKernel.Application.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Remove last element of an enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> elements)
        {
            var folders = elements.ToList();
            folders.Reverse();
            folders = folders.Skip(1).ToList();
            folders.Reverse();
            return folders;
        }

        /// <summary>
        /// Inserts an item to the enumerable first position
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<T> AddFirst<T>(this IEnumerable<T> items, T item) where T : notnull
        {
            if (items == default!)
                return Enumerable.Empty<T>();

            var itemsList = items.ToList();
            itemsList.Insert(0, item);

            return itemsList;
        }

        /// <summary>
        /// Sum time stamp
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TimeSpan SumTimeSpan<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
        {
            return source.Select(selector).Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
        }

        /// <summary>
        ///     Convert a string to lambda expression
        ///     Example => "Person.Child.Name" : x => x.Person.Child.Name
        /// </summary>
        /// <typeparam name="TAggregate"></typeparam>
        /// <param name="column"></param>
        /// <returns></returns>
        public static dynamic ToLambdaExpression<TAggregate>(this string column)
        {
            if (string.IsNullOrWhiteSpace(column))
                throw new ArgumentNullException(nameof(column));

            var props = column.Split('.');
            var type = typeof(TAggregate);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetProperty(prop);

                if (pi == null)
                    throw new ArgumentNullException(prop);

                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TAggregate), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            return lambda;
        }

        /// <summary>
        /// Returns if two elements are consecutive
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static bool AreConsecutive(this IEnumerable<int> elements)
        {
            return !elements.Select((i, j) => i - j).Distinct().Skip(1).Any();
        }

        #region Specifications

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAggregateRoot"></typeparam>
        /// <param name="items"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public static IEnumerable<TAggregateRoot> AllMatching<TAggregateRoot>(
            this IEnumerable<TAggregateRoot> items, ISpecification<TAggregateRoot> spec)
            where TAggregateRoot : class, IAggregateRoot
        {
            return items.Where(spec.SatisfiedBy().Compile());
        }

        #endregion

        #region Random

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        private static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(_ => Guid.NewGuid());
        }

        #endregion Random

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static IEnumerable<T> ContainsText<T>(this IEnumerable<T> query, string searchText)
        {
            Expression<Func<T, bool>> allExpressions = default!;
            foreach (var property in typeof(T).GetProperties().Where(t => t.PropertyType == typeof(string)))
            {
                var exp = GetExpressionContainsString<T>(property.Name, searchText);
                allExpressions = allExpressions == null ? exp : allExpressions.Or(exp);
            }

            if (allExpressions != null)
                query = query.Where(allExpressions.Compile());

            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static IEnumerable<T> FilterContainsProperties<T>(this IEnumerable<T> query, IEnumerable<FilterProperty> properties)
        {
            if (properties == default!)
                return query;

            query = properties.Where(f => !string.IsNullOrWhiteSpace(f.Value)).Aggregate(query,
                (current, filter) => current.Where(GetExpressionContainsString<T>(filter.Field, filter.Value).Compile()));

            return query;
        }

        private static Expression<Func<T, bool>> GetExpressionContainsString<T>(string propertyName, string propertyValue)
        {
            var propertyInfo = typeof(T).GetProperties().SingleOrDefault(t => t.Name.ToUpper() == propertyName.ToUpper());
            if (propertyInfo == null)
                throw new SharedKernelApplicationException($"Property {propertyName} not found");

            // Expression
            var parameterExp = Expression.Parameter(typeof(T), "type");
            var propertyExp = Expression.Property(parameterExp, propertyName);

            // Method
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            if (method == null)
                throw new SharedKernelApplicationException("Method Contains not found");

            // Value
            var someValue = Expression.Constant(propertyValue, typeof(string));

            var containsMethodExp = Expression.Call(propertyExp, method, someValue);
            return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
        }
    }
}
