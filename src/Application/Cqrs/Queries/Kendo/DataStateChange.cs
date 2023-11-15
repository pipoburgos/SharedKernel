namespace SharedKernel.Application.Cqrs.Queries.Kendo;

/// <summary> Paginación desde base de datos </summary>
public class DataStateChange
{
    /// <summary> The number of records to skip. </summary>
    public int? Skip { get; set; }

    /// <summary> The number of records to take. </summary>
    public int? Take { get; set; }

    /// <summary> The sort descriptors by which the data is sorted. </summary>
    public IEnumerable<SortDescriptor> Sort { get; set; } = null!;

    /// <summary> The group descriptors by which the data is grouped. </summary>
    public IEnumerable<GroupDescriptor>? Group { get; set; }

    /// <summary> The filter descriptor by which the data is filtered. </summary>
    public CompositeFilterDescriptor? Filter { get; set; }
}