using SharedKernel.Testing.Docker;

namespace SharedKernel.Integration.Tests.Hooks;

[CollectionDefinition("DockerHook")]
public class DockerHookCollection : ICollectionFixture<DockerComposeCmdHook>, IDisposable
{
    private readonly DockerComposeCmdHook _dockerHook;

    public DockerHookCollection(DockerComposeCmdHook dockerHook)
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