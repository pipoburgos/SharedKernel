namespace SharedKernel.Application.Cqrs.Queries.Kendo;

/// <summary> Ordenación de la paginación </summary>
public class SortDescriptor
{
    /// <summary> The field that is sorted. </summary>
    public string Field { get; set; } = null!;

    /// <summary>
    /// The sort direction. If no direction is set, the descriptor will be skipped during processing.
    ///
    /// The available values are:
    /// - `asc`
    /// - `desc`
    /// </summary>
    public string Dir { get; set; } = null!;
}