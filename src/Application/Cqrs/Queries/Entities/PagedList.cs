using SharedKernel.Application.Cqrs.Queries.Contracts;

namespace SharedKernel.Application.Cqrs.Queries.Entities;

/// <summary> A paged result. </summary>
/// <typeparam name="T"></typeparam>
public class PagedList<T> : IPagedList<T>
{
    private static PagedList<T> Instance => new(0, []);

    /// <summary> . </summary>
    public static PagedList<T> Empty() => Instance;

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