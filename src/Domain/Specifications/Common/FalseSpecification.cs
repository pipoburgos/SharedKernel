﻿namespace SharedKernel.Domain.Specifications.Common;

/// <inheritdoc />
/// <summary>
/// True specification
/// </summary>
/// <typeparam name="TEntity">Type of entity in this specification</typeparam>
public sealed class FalseSpecification<TEntity> : ISpecification<TEntity>
{
    #region Specification overrides

    /// <inheritdoc />
    /// <summary>
    /// <see cref="!: Specification{TEntity}" />
    /// </summary>
    public Expression<Func<TEntity, bool>> SatisfiedBy()
    {
        //Create "result variable" transform adhoc execution plan in prepared plan
        //for more info: http://geeks.ms/blogs/unai/2010/07/91/ef-4-0-performance-tips-1.aspx
        const bool result = false;

        Expression<Func<TEntity, bool>> trueExpression = t => result;
        return trueExpression;
    }

    #endregion
}