using SharedKernel.Application.Cqrs.Queries.DataTables;

namespace SharedKernel.Api.Grids.DataTables;

/// <summary> Datatables request. </summary>
public class DataTablesRequest
{
    /// <summary>  </summary>
    public DataTablesRequest(int draw, int start, int length, List<Column> columns, IEnumerable<DataTablesOrder> order,
        Search? search = default, IDictionary<string, object>? additionalParameters = default)
    {
        Draw = draw;
        Start = start;
        Length = length;
        Search = search;
        Columns = columns;
        Order = order;
        AdditionalParameters = additionalParameters ?? new Dictionary<string, object>();
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
    public IDictionary<string, object> AdditionalParameters { get; }
}
