namespace SharedKernel.Application.Cqrs.Queries.Entities;

/// <summary> Base filter for Dapper queries </summary>
public class PageOptions
{
    /// <summary>  </summary>
    public PageOptions() { }

    /// <summary> </summary>
    public PageOptions(int? skip, int? take, string? searchText, bool showDeleted, bool showOnlyDeleted,
        IEnumerable<Order> orders, IEnumerable<FilterProperty>? filterProperties)
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

    /// <summary>  </summary>
    public int? Take { get; set; }

    /// <summary>  </summary>
    public string? SearchText { get; set; }

    /// <summary>  </summary>
    public bool? ShowDeleted { get; set; }

    /// <summary>  </summary>
    public bool? ShowOnlyDeleted { get; set; }

    /// <summary>  </summary>
    public IEnumerable<Order> Orders { get; set; } = null!;

    /// <summary>  </summary>
    public IEnumerable<FilterProperty>? FilterProperties { get; set; }
}
