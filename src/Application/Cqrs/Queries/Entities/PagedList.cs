using System.Collections.Generic;
using SharedKernel.Application.Cqrs.Queries.Contracts;

namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    /// <summary>
    /// A paged result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : IPagedList<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRecords"></param>
        /// <param name="totalRecordsFiltered"></param>
        /// <param name="items"></param>
        public PagedList(int totalRecords, int totalRecordsFiltered, IEnumerable<T> items)
        {
            TotalRecords = totalRecords;
            TotalRecordsFiltered = totalRecordsFiltered;
            Items = items;
        }

        /// <summary>
        /// Total records before filtered
        /// </summary>
        public int TotalRecords { get; }

        /// <summary>
        /// Total records after filtered
        /// </summary>
        public int TotalRecordsFiltered { get; }

        /// <summary>
        /// Paged items
        /// </summary>
        public IEnumerable<T> Items { get; }
    }
}
