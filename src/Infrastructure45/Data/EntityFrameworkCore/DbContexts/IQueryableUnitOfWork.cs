using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts
{
    public interface IQueryableUnitOfWork : IUnitOfWorkAsync
    {
        DbSet<TAggregateRoot> SetAggregate<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;
    }
}