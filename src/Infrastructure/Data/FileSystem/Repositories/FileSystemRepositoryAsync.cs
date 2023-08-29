using SharedKernel.Application.Serializers;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.FileSystem.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public abstract class FileSystemRepositoryAsync<TAggregateRoot, TId> : FileSystemRepository<TAggregateRoot, TId>,
        ICreateRepositoryAsync<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TId>
    {
        /// <summary>  </summary>
        public FileSystemRepositoryAsync(IJsonSerializer jsonSerializer) : base(jsonSerializer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
        {
            // ReSharper disable once RedundantSuppressNullableWarningExpression
            using var outputFile = new StreamWriter(FileName(aggregate.Id!.ToString()!), false);
            await outputFile.WriteLineAsync(JsonSerializer.Serialize(aggregate));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
        {
            return Task.WhenAll(aggregates.Select(aggregateRoot => AddAsync(aggregateRoot, cancellationToken)));
        }
    }
}
