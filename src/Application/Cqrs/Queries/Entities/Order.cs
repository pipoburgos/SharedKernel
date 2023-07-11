namespace SharedKernel.Application.Cqrs.Queries.Entities;

/// <summary>  </summary>
public class Order
{
    /// <summary>  </summary>
    public Order(string field, bool? ascending = default)
    {
        Field = field;
        Ascending = ascending;
    }

    /// <summary>  </summary>
    public string Field { get; }

    /// <summary>  </summary>
    public bool? Ascending { get; }
}
