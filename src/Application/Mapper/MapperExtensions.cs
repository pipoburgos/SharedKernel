using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Application.Mapper;

/// <summary> Type adapter extensions for manage collections. </summary>
public static class MapperExtensions
{
    /// <summary> Project a type using a DTO. </summary>
    /// <typeparam name="TProjection">The dto projection</typeparam>
    /// <param name="aggregate">The source aggregate to project</param>
    /// <returns>The projected type</returns>
    public static TProjection MapTo<TProjection>(this object aggregate)
    {
        return MapperFactory.Create().MapTo<TProjection>(aggregate);
    }

    /// <summary> Projected a enumerable collection of items. </summary>
    /// <typeparam name="TProjection">The dto projection type</typeparam>
    /// <param name="items">the collection of entity items</param>
    /// <returns>Projected collection</returns>
    public static List<TProjection> MapTo<TProjection>(this IEnumerable<object> items)
    {
        return MapperFactory.Create().MapTo<List<TProjection>>(items);
    }

    /// <summary> Projected a enumerable collection of items. </summary>
    /// <typeparam name="TProjection">The dto projection type</typeparam>
    /// <param name="items">the collection of entity items</param>
    /// <returns>Projected collection</returns>
    public static IQueryable<TProjection> ProjectTo<TProjection>(this IQueryable items)
    {
        return MapperFactory.Create().ProjectTo<TProjection>(items);
    }
}
