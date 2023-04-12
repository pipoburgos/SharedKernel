using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System;
using System.Threading;
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            _dockerHook?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class DockerHook : IDisposable
    {
        private readonly ICompositeService _compositeService;

        public DockerHook()
        {
            var a = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile("./docker-compose.yml")
                .RemoveOrphans()
                //.WaitForPort("sql_server", "8038")
                //.WaitForPort("mongo", "22221")
                //.WaitForPort("redis", "22222")
                //.WaitForPort("smtp", "22224")
                //.WaitForPort("postgres", "22225")
                .Build();

            Thread.Sleep(TimeSpan.FromMinutes(1));

            _compositeService = a.Start();
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
