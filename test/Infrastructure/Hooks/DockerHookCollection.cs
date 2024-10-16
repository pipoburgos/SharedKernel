namespace SharedKernel.Integration.Tests.Hooks;

public sealed class DockerMockHook : IDisposable
{
    public void Dispose()
    {
    }
}

[CollectionDefinition("DockerHook")]
public class DockerHookCollection : ICollectionFixture<DockerMockHook>, IDisposable
{
    private readonly IDisposable _dockerHook;

    public DockerHookCollection(DockerMockHook dockerHook)
    {
        _dockerHook = dockerHook;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        _dockerHook.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}