using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System;
using System.Threading;

namespace SharedKernel.Testing.Docker
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
                .Build()
                .Start();

            Thread.Sleep(TimeSpan.FromSeconds(15));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            _compositeService.Stop();
            _compositeService?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
