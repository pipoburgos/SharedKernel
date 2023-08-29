using SharedKernel.Domain.Entities;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <param name="records"></param>
        /// <returns></returns>
        public static Task AddOrNothing<T, TId>(this DbSet<T> dbSet, CancellationToken cancellationToken,
            List<T> records) where T : class, IEntity<TId>
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
