namespace SharedKernel.Application.Cqrs.Queries.Entities;

/// <summary> . </summary>
public class ListItemBase<T> where T : notnull
{
    /// <summary> . </summary>
    public T Id { get; set; } = default!;

    /// <summary> . </summary>
    public bool Deleted { get; set; }
}
