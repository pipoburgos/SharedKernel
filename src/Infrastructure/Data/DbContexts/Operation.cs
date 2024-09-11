namespace SharedKernel.Infrastructure.Data.DbContexts;


/// <summary> . </summary>
public interface IOperation
{
    /// <summary> . </summary>
    public Crud Crud { get; set; }

    /// <summary> . </summary>
    public Action CommitMethod { get; }

    /// <summary> . </summary>
    public Action RollbackMethod { get; }

    /// <summary> . </summary>
    public IAggregateRoot AggregateRoot { get; set; }
}

/// <summary> . </summary>
public class Operation<T, TId> : IOperation where T : class, IAggregateRoot<TId> where TId : notnull
{
    /// <summary> . </summary>
    public Operation(Crud crud, T aggregateRoot, Action commitMethod, Action rollbackMethod)
    {
        Crud = crud;
        AggregateRoot = aggregateRoot;
        CommitMethod = commitMethod;
        RollbackMethod = rollbackMethod;
    }

    /// <summary> . </summary>
    public Crud Crud { get; set; }

    /// <summary> . </summary>
    public IAggregateRoot AggregateRoot { get; set; }

    /// <summary> . </summary>
    public Action CommitMethod { get; }

    /// <summary> . </summary>
    public Action RollbackMethod { get; }
}