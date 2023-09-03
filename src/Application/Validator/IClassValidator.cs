namespace SharedKernel.Application.Validator;

/// <summary> The entity validator base contract. </summary>
public interface IClassValidator<in T>
{
    /// <summary>  </summary>
    List<ValidationFailure> ValidateList(T item);

    /// <summary>  </summary>
    void Validate(T item);

    /// <summary>  </summary>
    Task<List<ValidationFailure>> ValidateListAsync(T item, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task ValidateAsync(T item, CancellationToken cancellationToken);
}
