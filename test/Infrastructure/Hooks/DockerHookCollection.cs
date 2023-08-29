using SharedKernel.Testing.Docker;
using Xunit;

namespace SharedKernel.Integration.Tests.Hooks;

[CollectionDefinition("DockerHook")]
public class DockerHookCollection : ICollectionFixture<DockerHook>, IDisposable
{
    private readonly DockerHook _dockerHook;

    public DockerHookCollection(DockerHook dockerHook)
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