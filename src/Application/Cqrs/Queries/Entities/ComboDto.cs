namespace SharedKernel.Application.Cqrs.Queries.Entities;

/// <summary> Combo item. </summary>
public class ComboDto<TId> where TId : notnull
{
    /// <summary> Combo value. </summary>
    public TId Value { get; set; } = default!;

    /// <summary> Combo text. </summary>
    public string Text { get; set; } = null!;
}
