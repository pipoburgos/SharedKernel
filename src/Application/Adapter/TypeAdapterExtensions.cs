using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Application.Adapter
{
    /// <summary>
    /// Type adapter extensions for manage collections
    /// </summary>
    public static class TypeAdapterExtensions
    {
        /// <summary>
        /// Project a type using a DTO
        /// </summary>
        /// <typeparam name="TProjection">The dto projection</typeparam>
        /// <param name="aggregate">The source aggregate to project</param>
        /// <returns>The projected type</returns>
        public static TProjection MapTo<TProjection>(this object aggregate)
        {
            return TypeAdapterFactory.Create().MapTo<TProjection>(aggregate);
        }

        /// <summary>
        /// projected a enumerable collection of items
        /// </summary>
        /// <typeparam name="TProjection">The dtop projection type</typeparam>
        /// <param name="items">the collection of entity items</param>
        /// <returns>Projected collection</returns>
        public static List<TProjection> MapTo<TProjection>(this IEnumerable<object> items)
        {
            return TypeAdapterFactory.Create().MapTo<List<TProjection>>(items);
        }

        /// <summary>
        /// projected a enumerable collection of items
        /// </summary>
        /// <typeparam name="TProjection">The dtop projection type</typeparam>
        /// <param name="items">the collection of entity items</param>
        /// <returns>Projected collection</returns>
        public static IQueryable<TProjection> ProjectTo<TProjection>(this IQueryable items)
        {
            return TypeAdapterFactory.Create().ProjectTo<TProjection>(items);
        }
    }
}
