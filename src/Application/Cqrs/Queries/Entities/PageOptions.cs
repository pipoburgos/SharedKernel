namespace SharedKernel.Application.Cqrs.Queries.Entities;

/// <summary> Base filter for Dapper queries </summary>
public class PageOptions
{
    /// <summary> . </summary>
    public PageOptions() { }

    /// <summary> </summary>
    public PageOptions(int? skip = null, int? take = null, string? searchText = null, bool? showDeleted = null,
        bool? showOnlyDeleted = null, Order[]? orders = null, FilterProperty[]? filterProperties = null)
    {
        Skip = skip;
        Take = take;
        SearchText = searchText;
        ShowDeleted = showDeleted;
        ShowOnlyDeleted = showOnlyDeleted;
        Orders = orders;
        FilterProperties = filterProperties;
    }

    /// <summary> </summary>
    public int? Skip { get; set; }

    /// <summary> . </summary>
    public int? Take { get; set; }

    /// <summary> . </summary>
    public string? SearchText { get; set; }

    /// <summary> . </summary>
    public bool? ShowDeleted { get; set; }

    /// <summary> . </summary>
    public bool? ShowOnlyDeleted { get; set; }

    /// <summary> . </summary>
    public Order[]? Orders { get; set; }

    /// <summary> . </summary>
    public FilterProperty[]? FilterProperties { get; set; }
}
