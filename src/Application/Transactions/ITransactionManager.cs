namespace SharedKernel.Application.Transactions;

public interface ITransactionManager
{
    Task ExecuteAsync(Func<Task> operation);
}
