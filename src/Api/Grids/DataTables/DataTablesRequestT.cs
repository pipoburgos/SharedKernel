using SharedKernel.Application.Cqrs.Queries.DataTables;

namespace SharedKernel.Api.Grids.DataTables;

/// <summary> Datatables request. </summary>
public class DataTablesRequest<T>
{
    /// <summary>  </summary>
    public DataTablesRequest(T filter, int draw, int start, int length, List<Column> columns,
        IEnumerable<DataTablesOrder> order, Search? search = default)
    {
        Filter = filter;
        Draw = draw;
        Start = start;
        Length = length;
        Search = search;
        Columns = columns;
        Order = order;
    }

    /// <summary>  </summary>
    public int Draw { get; }

    /// <summary>  </summary>
    public int Start { get; }

    /// <summary>  </summary>
    public int Length { get; }

    /// <summary>  </summary>
    public Search? Search { get; }

    /// <summary>  </summary>
    public List<Column> Columns { get; }

    /// <summary>  </summary>
    public IEnumerable<DataTablesOrder> Order { get; }

    /// <summary>  </summary>
    public T Filter { get; }
}