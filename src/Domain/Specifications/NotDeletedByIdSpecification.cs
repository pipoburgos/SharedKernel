using System;
using System.Linq.Expressions;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Domain.Specifications
{
    public class NotDeletedByIdSpecification<T, TKey> : Specification<T>
        where T : class, IEntity<TKey>, IEntityAuditableLogicalRemove
    {
        private TKey Key { get; }

        public NotDeletedByIdSpecification(TKey key)
        {
            Key = key;
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return new AndSpecification<T>(new NotDeletedSpecification<T>(), new EntityByIdSpecification<T, TKey>(Key))
                .SatisfiedBy();
        }
    }
}
