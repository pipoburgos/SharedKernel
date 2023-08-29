namespace SharedKernel.Application.Cqrs.Queries.Entities;

/// <summary> Combo item. </summary>
public class ComboDto<TKey> where TKey : notnull
{
    /// <summary> Combo value. </summary>
    public TKey Value { get; set; } = default!;

    /// <summary> Combo text. </summary>
    public string Text { get; set; } = null!;
}
