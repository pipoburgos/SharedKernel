namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    public interface IReadSpecificationRepositoryAsync<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<List<TAggregateRoot>> WhereAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TAggregateRoot> SingleAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TAggregateRoot> SingleOrDefaultAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);
    }
}