using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.FileSystem.Repositories
{
    public abstract class FileSystemRepositoryAsync<TAggregateRoot, TKey> : FileSystemRepository<TAggregateRoot, TKey>, IRepositoryAsync<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        public  Task AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
        {
            using var outputFile = new StreamWriter(FileName(aggregate.Id.ToString()), false);
            return outputFile.WriteLineAsync(JsonConvert.SerializeObject(aggregate));
        }

        public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> GetByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> GetDeleteByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TAggregateRoot entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<TAggregateRoot> entities, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<TAggregateRoot>> WhereAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> SingleAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TAggregateRoot> SingleOrDefaultAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> RollbackAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
