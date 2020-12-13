using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Entities;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Extensions
{
    public static class DbSetExtensions
    {
        public static Task AddOrNothing<T, TKey>(this DbSet<T> dbSet, CancellationToken cancellationToken,
            List<T> records) where T : class, IEntity<TKey>
        {
            var tasks = (from data in records
                         let exists = dbSet.AsNoTracking().Any(x => x.Id.Equals(data.Id))
                         where !exists
                         select dbSet.AddAsync(data, cancellationToken).AsTask())
                .Cast<Task>();

            return Task.WhenAll(tasks);
        }
    }
}
