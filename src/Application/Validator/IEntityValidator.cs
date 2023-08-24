namespace SharedKernel.Application.Validator;

/// <summary> The entity validator base contract. </summary>
public interface IEntityValidator<in TEntity>
{
    /// <summary>  </summary>
    List<ValidationFailure> ValidateList(TEntity item);

    /// <summary>  </summary>
    void Validate(TEntity item);

    /// <summary>  </summary>
    Task<List<ValidationFailure>> ValidateListAsync(TEntity item, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task ValidateAsync(TEntity item, CancellationToken cancellationToken);
}
