using System.Collections.Generic;

namespace SharedKernel.Application.Cqrs.Queries.Contracts
{
    public interface IPagedList<out T>
    {
        int TotalRecords { get; }

        int TotalRecordsFiltered { get; }

        IEnumerable<T> Items { get; }
    }
}