namespace SharedKernel.Domain.Specifications;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class NotDeletedSpecification<T> : ISpecification<T> where T : class , IEntityAuditableLogicalRemove
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Expression<Func<T, bool>> SatisfiedBy()
    {
        return audi => !audi.DeletedAt.HasValue;
    }
}