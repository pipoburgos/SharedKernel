using SharedKernel.Application.Cqrs.Queries.Contracts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Application.Extensions;
using SharedKernel.Application.Mapper;
using SharedKernel.Domain.Extensions;
// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

namespace SharedKernel.Infrastructure.Data.Queryable;

/// <summary>
/// Queryable extensions
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="mustFilter"></param>
    /// <param name="expr"></param>
    /// <returns></returns>
    public static IQueryable<T> Where<T>(this IQueryable<T> queryable, bool mustFilter, Expression<Func<T, bool>> expr)
    {
        return !mustFilter ? queryable : queryable.Where(expr);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="specs"></param>
    /// <returns></returns>
    public static IQueryable<T> Where<T>(this IQueryable<T> queryable, IEnumerable<ISpecification<T>> specs)
    {
        if (specs == default!)
            return queryable;

        var properties = specs.ToList();
        if (!properties.Any())
            return queryable;

        ISpecification<T> all = new DirectSpecification<T>(e => true);
        var final = properties.Aggregate(all, (current, dtoSpecification) => current.And(dtoSpecification));
        return queryable.Where(final.SatisfiedBy());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="searchText"></param>
    /// <returns></returns>
    public static IQueryable<T> Where<T>(this IQueryable<T> queryable, string? searchText)
    {
        if (searchText == default || string.IsNullOrWhiteSpace(searchText))
            return queryable;

        var searchTextSpec = new ContainsTextSpecification<T>(searchText);

        return queryable.Where(searchTextSpec.SatisfiedBy());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="filterProperties"></param>
    /// <returns></returns>
    public static IQueryable<T> Where<T>(this IQueryable<T> queryable, IEnumerable<FilterProperty>? filterProperties)
    {
        if (filterProperties == default!)
            return queryable;

        var properties = filterProperties.ToList();
        if (!properties.Any())
            return queryable;

        var propertiesSpec = new AllPropertiesValueMatchesSpecification<T>(
            properties.Select(p => new Property(p.Field, p.Value, (Operator?)p.Operator, p.IgnoreCase ?? true)));

        return queryable.Where(propertiesSpec.SatisfiedBy());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="showDeleted"></param>
    /// <param name="showOnlyDeleted"></param>
    /// <returns></returns>
    public static IQueryable<T> Where<T>(this IQueryable<T> queryable, bool showDeleted, bool showOnlyDeleted)
    {
        if (!typeof(IEntityAuditableLogicalRemove).IsAssignableFrom(typeof(T)))
            return queryable;

        if (!showDeleted)
        {
            return queryable
                .Cast<IEntityAuditableLogicalRemove>()
                .Where(new NotDeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy())
                .Cast<T>();
        }

        if (showOnlyDeleted)
        {
            return queryable
                .Cast<IEntityAuditableLogicalRemove>()
                .Where(new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy())
                .Cast<T>();
        }

        return queryable;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IQueryable<TResult> MapToDto<T, TResult>(this IQueryable<T> queryable,
        Expression<Func<T, TResult>>? selector)
    {
        if (typeof(T) == typeof(TResult))
            return queryable.Cast<TResult>();

        return selector == default ? queryable.ProjectTo<TResult>() : queryable.Select(selector);
    }

    /// <summary>  </summary>
    public static IPagedList<T> ToPagedList<T>(this IQueryable<T> queryable, PageOptions pageOptions) where T : class
    {
        var query = queryable
            .Where(pageOptions.FilterProperties)
            .Where(pageOptions.SearchText);

        var total = query.Count();

        if (total == default)
            return PagedList<T>.Empty();

        var elements = query.OrderAndPaged(pageOptions).ToList();

        return new PagedList<T>(total, elements);
    }

    #region OrderAndPaged

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="pageOptions"></param>
    /// <returns></returns>
    public static IQueryable<T> OrderAndPaged<T>(this IQueryable<T> queryable, PageOptions pageOptions)
    {
        return queryable
            .Order(pageOptions.Orders)
            .Skip(pageOptions.Skip ?? 0)
            .Take(pageOptions.Take ?? int.MaxValue);
    }

    /// <summary>
    /// Apply IQueryable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="sortedColumns"></param>
    /// <returns></returns>
    private static IQueryable<T> Order<T>(this IQueryable<T> query, IEnumerable<Order>? sortedColumns)
    {
        var sortedList = sortedColumns?.ToList();
        if (sortedList == default || !sortedList.Any())
        {
            if (new IsClassTypeSpecification<T>().SatisfiedBy().Compile()(default!))
                throw new ArgumentException(nameof(sortedColumns));

            sortedList = new List<Order> { new Order(default, true) };
        }

        var firstColumn = sortedList.First();
        query = ApplyOrderBy(query, firstColumn.Ascending ?? true, firstColumn.Field?.CapitalizeFirstLetter());

        foreach (var column in sortedList.Skip(1))
        {
            query = ApplyThenBy(query, column.Ascending ?? true, column.Field?.CapitalizeFirstLetter());
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
        string? propertyName)
    {
        var expression = ExpressionHelper.GetLambdaExpressions<TResult>(propertyName);
        if (expression == null)
            return source;

        var order = ascending
            ? nameof(global::System.Linq.Queryable.OrderBy)
            : nameof(global::System.Linq.Queryable.OrderByDescending);

        var resultExpression = Expression.Call(
            typeof(global::System.Linq.Queryable),
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
        string? propertyName)
    {
        var expression = ExpressionHelper.GetLambdaExpressions<TResult>(propertyName);
        if (expression == null)
            return source;

        var order = ascending
            ? nameof(global::System.Linq.Queryable.ThenBy)
            : nameof(global::System.Linq.Queryable.ThenByDescending);

        var resultExpression = Expression.Call(
            typeof(global::System.Linq.Queryable),
            order,
            new[] { typeof(TResult), expression.ReturnType },
            source.Expression,
            Expression.Quote(expression));

        return source.Provider.CreateQuery<TResult>(resultExpression);
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
}