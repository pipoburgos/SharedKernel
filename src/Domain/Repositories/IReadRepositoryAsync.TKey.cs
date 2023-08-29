namespace SharedKernel.Domain.Repositories;

/// <summary>  </summary>
public interface IReadRepositoryAsync<TAggregate, in TId> where TAggregate : IAggregateRoot
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<TAggregate> GetByIdAsync(TId key, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<TAggregate> GetDeleteByIdAsync(TId key, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<bool> AnyAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<bool> NotAnyAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<int> CountAsync(CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<bool> AnyAsync(TId key, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<bool> NotAnyAsync(TId key, CancellationToken cancellationToken);
}