using SharedKernel.Application.Cqrs.Queries.Entities;

namespace SharedKernel.Application.Extensions;

/// <summary> . </summary>
public static class EnumerableExtensions
{
    /// <summary> . </summary>
    public static IEnumerable<T> FilterContainsProperties<T>(this IEnumerable<T> query, IEnumerable<FilterProperty> properties)
    {
        if (properties == default!)
            return query;

        query = properties.Where(f => !string.IsNullOrWhiteSpace(f.Value)).Aggregate(query,
            (current, filter) => current.Where(Domain.Extensions.EnumerableExtensions
                .GetExpressionContainsString<T>(filter.Field, filter.Value).Compile()));

        return query;
    }
}