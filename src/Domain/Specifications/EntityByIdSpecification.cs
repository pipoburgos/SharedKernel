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
    /// <typeparam name="TKey"></typeparam>
    public class EntityByIdSpecification<T, TKey> : Specification<T> where T : class, IEntity<TKey>
    {
        private readonly TKey _key;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public EntityByIdSpecification(TKey key)
        {
            _key = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return _ => _.Id.Equals(_key);
        }
    }
}
