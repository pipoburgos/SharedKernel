using Newtonsoft.Json;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.FileSystem.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class FileSystemRepositoryAsync<TAggregateRoot, TKey> : FileSystemRepository<TAggregateRoot, TKey>,
        ICreateRepositoryAsync<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
        {
            await using var outputFile = new StreamWriter(FileName(aggregate.Id.ToString()), false);
            await outputFile.WriteLineAsync(JsonConvert.SerializeObject(aggregate));
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
