namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    public interface IUpdateRepository<in TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        void Update(TAggregate aggregate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        void UpdateRange(IEnumerable<TAggregate> aggregates);
    }
}