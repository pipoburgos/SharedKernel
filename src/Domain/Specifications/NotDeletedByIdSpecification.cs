namespace SharedKernel.Domain.Specifications
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class NotDeletedByIdSpecification<T, TKey> : Specification<T>
        where T : class, IEntity<TKey>, IEntityAuditableLogicalRemove
    {
        private TKey Key { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public NotDeletedByIdSpecification(TKey key)
        {
            Key = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            return new AndSpecification<T>(new NotDeletedSpecification<T>(), new EntityByIdSpecification<T, TKey>(Key))
                .SatisfiedBy();
        }
    }
}
