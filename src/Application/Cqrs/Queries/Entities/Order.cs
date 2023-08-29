namespace SharedKernel.Application.Cqrs.Queries.Entities;

/// <summary>  </summary>
public class Order
{
    /// <summary>  </summary>
    public Order(string? field, bool? ascending = default)
    {
        Field = field;
        Ascending = ascending;
    }

    /// <summary> Is optional because can order primitives lists. </summary>
    public string? Field { get; }

    /// <summary>  </summary>
    public bool? Ascending { get; }
}
