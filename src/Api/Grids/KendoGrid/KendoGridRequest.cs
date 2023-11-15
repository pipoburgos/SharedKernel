using SharedKernel.Application.Cqrs.Queries.Kendo;

namespace SharedKernel.Api.Grids.KendoGrid;

/// <summary>
/// 
/// </summary>
public class KendoGridRequest<T>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="state"></param>
    public KendoGridRequest(T filter, DataStateChange state)
    {
        Filter = filter;
        State = state;
    }

    /// <summary>
    /// 
    /// </summary>
    public T Filter { get; }

    /// <summary>
    /// 
    /// </summary>
    public DataStateChange State { get; }
}