using Microsoft.EntityFrameworkCore;
using SharedKernel.Testing.Acceptance.Exceptions;

namespace SharedKernel.Testing.Acceptance;

public class DatabaseManager : IAsyncDisposable
{
    public DbContext Context { get; private set; }

    public DatabaseManager(DbContext context)
    {
        Context = context;
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
    }

    public T AddAndSaveChanges<T>(T entity)
    {
        Context.Add(entity!);
        SaveChanges();
        return entity;
    }

    public virtual void SaveChanges()
    {
        new SaveChangesExceptionHandler().SaveChanges(() => Context.SaveChanges());
    }

    public void DisposeContext()
    {
        Context.Dispose();
    }
}