using System;
using System.Linq.Expressions;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Domain.Specifications
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DeletedSpecification<T> : ISpecification<T> where T : class , IEntityAuditableLogicalRemove
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
}
