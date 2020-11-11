using System;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts
{
    public interface IReadContext : IDisposable
    {
        /// <summary>
        /// Returns a IDbSet instance for access to entities of the given type in the context,
        /// the ObjectStateManager, and the underlying store.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets the connection
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection { get; }
    }
}
