using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System;
using Xunit;

namespace SharedKernel.Integration.Tests.Docker
{
    [CollectionDefinition("DockerHook")]
    public class DockerHookCollection : ICollectionFixture<DockerHook>, IDisposable
    {
        private readonly DockerHook _dockerHook;
        public DockerHookCollection(DockerHook dockerHook)
        {
            _dockerHook = dockerHook;
        }

        public void Dispose()
        {
            _dockerHook.Dispose();
        }
    }

    public class DockerHook : IDisposable
    {
        private readonly ICompositeService _compositeService;

        public DockerHook()
        {
            _compositeService = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile("./docker-compose.yml")
                .RemoveOrphans()
                .Build()
                .Start();
        }

        public void Dispose()
        {
            _compositeService.Stop();
            _compositeService?.Dispose();
        }
    }
}
