namespace SharedKernel.Application.Cqrs.Queries.Kendo;

/// <summary> Agrupación de la consulta </summary>
public class GroupDescriptor
{
    /// <summary> The field that is sorted. </summary>
    public string Field { get; set; }

    /// <summary>
    /// The sort direction. If no direction is set, the descriptor will be skipped during processing.
    ///
    /// The available values are:
    /// - `asc`
    /// - `desc`
    /// </summary>
    public string Dir { get; set; }
}