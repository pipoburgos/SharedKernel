using System;
using System.Linq.Expressions;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Domain.Specifications
{
    public class EntityByIdSpecification<T, TKey> : Specification<T> where T : class, IEntity<TKey>
    {
        private readonly TKey _key;

        public EntityByIdSpecification(TKey key)
        {
            _key = key;
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return _ => _.Id.Equals(_key);
        }
    }
}
