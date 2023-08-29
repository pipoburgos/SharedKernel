namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    public interface IReadRepository<out TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TAggregate? GetById<TId>(TId key);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Any();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool NotAny();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Any<TId>(TId key);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        bool NotAny<TId>(TId key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IReadRepository<out TAggregate, in TId> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TAggregate GetById(TId key);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Any();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool NotAny();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Any(TId key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool NotAny(TId key);
    }
}