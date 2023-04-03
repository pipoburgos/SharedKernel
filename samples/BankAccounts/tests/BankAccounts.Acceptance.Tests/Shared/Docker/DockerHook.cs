using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace BankAccounts.Acceptance.Tests.Shared.Docker
{
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
                .WaitForPort("sql_server", "8765")
                .Build();

            Thread.Sleep(TimeSpan.FromSeconds(20));

            _compositeService = a.Start();

            //_compositeService = new Builder()
            //    .UseContainer()
            //    .UseCompose()
            //    .FromFile("./docker-compose.yml")
            //    .RemoveOrphans()
            //    .WaitForPort("sql_server", "8765")
            //    .Build()
            //    .Start();
        }

        public void Dispose()
        {
            _compositeService?.Stop();
            _compositeService?.Dispose();
        }
    }
}
