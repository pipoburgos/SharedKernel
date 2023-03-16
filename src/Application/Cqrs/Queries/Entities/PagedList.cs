using SharedKernel.Application.Cqrs.Queries.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    /// <summary> A paged result. </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : IPagedList<T>
    {
        /// <summary>  </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PagedList<T> Empty() => new(0, Enumerable.Empty<T>());

        /// <summary> Constructor. </summary>
        /// <param name="totalRecordsFiltered"></param>
        /// <param name="items"></param>
        public PagedList(int totalRecordsFiltered, IEnumerable<T> items)
        {
            TotalRecords = totalRecordsFiltered;
            TotalRecordsFiltered = totalRecordsFiltered;
            Items = items;
        }

        /// <summary> Total records before filtered. </summary>
        public int TotalRecords { get; }

        /// <summary> Total records after filtered. </summary>
        public int TotalRecordsFiltered { get; }

        /// <summary> Paged items. </summary>
        public IEnumerable<T> Items { get; }
    }
}
