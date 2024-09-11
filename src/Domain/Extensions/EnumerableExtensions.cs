namespace SharedKernel.Domain.Extensions;

/// <summary> Enumerable extensions. </summary>
public static class EnumerableExtensions
{
    /// <summary>Filters a sequence of values based on a predicate.</summary>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to filter.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements from the input sequence that satisfy the condition.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return source.Where(predicate);
    }

    /// <summary>Projects each element of a sequence into a new form.</summary>
    /// <param name="source">A sequence of values to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are the result of invoking the transform function on each element of <paramref name="source" />.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
    public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        return source.Select(selector);
    }

    /// <summary>Projects each element of a sequence to an <see cref="T:System.Collections.Generic.IEnumerable`1" /> and flattens the resulting sequences into one sequence.</summary>
    /// <param name="source">A sequence of values to project.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TResult">The type of the elements of the sequence returned by <paramref name="selector" />.</typeparam>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
    public static IEnumerable<TResult> FlatMap<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, IEnumerable<TResult>> selector)
    {
        return source.SelectMany(selector);
    }

    /// <summary>Sorts the elements of a sequence in ascending order according to a key.</summary>
    /// <param name="source">A sequence of values to order.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="reverse"></param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted according to a key.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
    public static IOrderedEnumerable<TSource> Sort<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector, bool reverse = false)
    {
        return reverse ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }

    /// <summary> Remove last element of an enumerable. </summary>
    public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> elements)
    {
        var folders = elements.ToList();
        folders.Reverse();
        folders = folders.Skip(1).ToList();
        folders.Reverse();
        return folders;
    }

    /// <summary> Inserts an item to the enumerable first position. </summary>
    public static IEnumerable<T> AddFirst<T>(this IEnumerable<T> items, T item) where T : notnull
    {
        if (items == default!)
            // ReSharper disable once UseCollectionExpression
            return Enumerable.Empty<T>();

        var itemsList = items.ToList();
        itemsList.Insert(0, item);

        return itemsList;
    }

    /// <summary> Sum time stamp. </summary>
    public static TimeSpan SumTimeSpan<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
    {
        return source.Select(selector).Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
    }

    /// <summary>
    ///     Convert a string to lambda expression
    ///     Example => "Person.Child.Name" : x => x.Person.Child.Name
    /// </summary>
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

    /// <summary> Returns if two elements are consecutive. </summary>
    public static bool AreConsecutive(this IEnumerable<int> elements)
    {
        return !elements.Select((i, j) => i - j).Distinct().Skip(1).Any();
    }

    #region Specifications

    /// <summary> . </summary>
    public static IEnumerable<TAggregateRoot> AllMatching<TAggregateRoot>(
        this IEnumerable<TAggregateRoot> items, ISpecification<TAggregateRoot> spec)
        where TAggregateRoot : class, IAggregateRoot
    {
        return items.Where(spec.SatisfiedBy().Compile());
    }

    #endregion

    #region Random

    /// <summary> . </summary>
    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    private static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(_ => Guid.NewGuid());
    }

    #endregion Random

    /// <summary> . </summary>
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

    /// <summary> . </summary>
    public static Expression<Func<T, bool>> GetExpressionContainsString<T>(string propertyName, string propertyValue)
    {
        var propertyInfo = typeof(T).GetProperties().SingleOrDefault(t => string.Equals(t.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));
        if (propertyInfo == null)
            throw new Exception($"Property {propertyName} not found");

        // Expression
        var parameterExp = Expression.Parameter(typeof(T), "type");
        var propertyExp = Expression.Property(parameterExp, propertyName);

        // Method
        var method = typeof(string).GetMethod("Contains", [typeof(string)]);
        if (method == null)
            throw new Exception("Method Contains not found");

        // Value
        var someValue = Expression.Constant(propertyValue, typeof(string));

        var containsMethodExp = Expression.Call(propertyExp, method, someValue);
        return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
    }

    /// <summary> Join on multiple enumerables by precondition. </summary>
    public static IEnumerable<IEnumerable<T>> Union<T>(this IOrderedEnumerable<T> source,
        Func<T, T, bool> isPrevious)
    {
        var sourceList = source.ToList();

        var result = new List<List<T>>();

        foreach (var item in sourceList)
        {
            var previous = sourceList.SingleOrDefault(x => isPrevious(x, item));
            if (previous == null)
            {
                result.Add([item]);
                continue;
            }

            var list = result.SingleOrDefault(x => x.Any(v => v != null && v.Equals(previous)));

            if (list == null)
                throw new Exception("Previous list not found ");

            list.Add(item);
        }

        return result;
    }
}