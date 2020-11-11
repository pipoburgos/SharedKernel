using System.Collections.Generic;
using SharedKernel.Application.Cqrs.Queries.Contracts;

namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : IPagedList<T>
    {
        public PagedList(int totalRecords, int totalRecordsFiltered, IEnumerable<T> items)
        {
            TotalRecords = totalRecords;
            TotalRecordsFiltered = totalRecordsFiltered;
            Items = items;
        }

        public int TotalRecords { get; }
        public int TotalRecordsFiltered { get; }
        public IEnumerable<T> Items { get; }
    }
}
