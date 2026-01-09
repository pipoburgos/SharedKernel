namespace SharedKernel.Integration.Tests.Cqrs.Commands;

public class SaveValueSingletonService
{
    private readonly Lock _lock;

    public SaveValueSingletonService()
    {
        _lock = new Lock();
    }

    public int Id { get; private set; }

    public void SetId(int id)
    {
        lock (_lock)
        {
            Id = id;
        }
    }
}
