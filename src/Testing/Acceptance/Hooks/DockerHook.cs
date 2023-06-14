using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System;

namespace SharedKernel.Testing.Acceptance.Hooks
{
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
                //.WaitForPort("postgres", "54321", 30_000)
                .Build()
                .Start();
        }

        public void Dispose()
        {
            _compositeService?.Stop();
            _compositeService?.Dispose();
        }
    }
}
