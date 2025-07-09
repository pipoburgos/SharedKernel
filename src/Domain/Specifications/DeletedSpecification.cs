﻿namespace SharedKernel.Domain.Specifications;

/// <summary> Delete specification for entities that implement. </summary>
/// <typeparam name="T"></typeparam>
public class DeletedSpecification<T> : ISpecification<T> where T : class, IEntityAuditableLogicalRemove
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Expression<Func<T, bool>> SatisfiedBy()
    {
        return audi => audi.DeletedAt.HasValue;
    }
}