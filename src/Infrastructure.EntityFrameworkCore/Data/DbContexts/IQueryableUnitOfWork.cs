using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQueryableUnitOfWork : IUnitOfWorkAsync
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAggregateRoot"></typeparam>
        /// <returns></returns>
        DbSet<TAggregateRoot> SetAggregate<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;
    }
}