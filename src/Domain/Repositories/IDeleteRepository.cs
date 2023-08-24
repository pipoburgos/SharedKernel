namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    public interface IDeleteRepository<in TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        void Remove(TAggregate aggregate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        void RemoveRange(IEnumerable<TAggregate> aggregate);
    }
}