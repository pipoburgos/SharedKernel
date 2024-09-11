namespace SharedKernel.Infrastructure.Data.DbContexts;


/// <summary> . </summary>
public interface IOperationAsync
{
    /// <summary> . </summary>
    public Crud Crud { get; set; }

    /// <summary> . </summary>
    public Func<Task> CommitMethodAsync { get; }

    /// <summary> . </summary>
    public Func<Task> RollbackMethodAsync { get; }

    /// <summary> . </summary>
    public IAggregateRoot AggregateRoot { get; set; }
}

/// <summary> . </summary>
public class OperationAsync<T, TId> : IOperationAsync where T : class, IAggregateRoot<TId> where TId : notnull
{
    /// <summary> . </summary>
    public OperationAsync(Crud crud, T aggregateRoot, Func<Task> commitMethodAsync, Func<Task> rollbackMethodAsync)
    {
        Crud = crud;
        AggregateRoot = aggregateRoot;
        CommitMethodAsync = commitMethodAsync;
        RollbackMethodAsync = rollbackMethodAsync;
    }

    /// <summary> . </summary>
    public Crud Crud { get; set; }

    /// <summary> . </summary>
    public IAggregateRoot AggregateRoot { get; set; }

    /// <summary> . </summary>
    public Func<Task> CommitMethodAsync { get; }

    /// <summary> . </summary>
    public Func<Task> RollbackMethodAsync { get; }
}